using Microsoft.AspNetCore.Hosting;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Data.Entity;
using System.Security.Policy;

namespace Petsitter.Repositories
{
    public class CsFacingSitterRepo
    {
        private readonly PetsitterContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CsFacingSitterRepo(PetsitterContext db)
        {
            _db = db;
        }

        public IQueryable<SitterVM> GetAllSitterVMs()
        {
            SitterRepos sRepos = new SitterRepos(_db, _webHostEnvironment);

            // Get all sitters and convert to list.
            var allSitters = (from s in _db.Sitters.Include(s=> s.Availabilities)
                             join u in _db.Users
                                    on s.UserId equals u.UserId
                           
                             select new SitterVM
                             {
                                 SitterId = s.SitterId,
                                 FirstName = u.FirstName,
                                 Rate = (decimal)s.RatePerPetPerDay,
                                 ProfileBio = s.ProfileBio,
                                 ProfileImage = u.ProfileImage,
                                 City = u.City,
                                 AvgRating = (double)_db.Bookings.Where(b => b.SitterId == s.SitterId).Average(b => b.Rating),
                                 petTypes = _db.Sitters.Where(b => b.SitterId == s.SitterId).SelectMany(s => s.PetTypes).Select(p => p.PetType1).ToList(),
                                 availabilities = s.Availabilities.ToList(),
                                 Reviews = sRepos.GetReviews(s.SitterId).ToList(),
                                 }).ToList();


            // Add availabilities.
            AvailabilityRepo availabilityRepo = new AvailabilityRepo(_db);
            foreach (var sitter in allSitters)
            {
                if (sitter.availabilities != null)
                {
                    sitter.availableDates = availabilityRepo.GetAvailableDates(sitter.availabilities);
                }
            }

            return allSitters.AsQueryable();
        }

        public SitterVM GetSitterVM(int sitterID)
        {
            return GetAllSitterVMs().Where(s => s.SitterId == sitterID).FirstOrDefault();
        }





        public User getUserById(int sitterId)
        {
            var user = (from s in _db.Sitters
                        join u in _db.Users on s.UserId equals u.UserId
                        where s.SitterId == sitterId
                        select u).FirstOrDefault();

            return user;
        }


    }
}
