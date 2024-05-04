using Microsoft.AspNetCore.Hosting;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Data.Entity;
using System.Drawing;

namespace Petsitter.Repositories
{
    public class ReviewRepo
    {
        PetsitterContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ReviewRepo(PetsitterContext context)
        {
            _db = context;
        }


        public Tuple<int, string, bool> UpdateReview(CreateReviewVM createReviewVM)
        {
            string message;
            bool success;

            try
            {
                Booking bookReview = _db.Bookings.Where(b => b.BookingId == createReviewVM.BookingId).FirstOrDefault();
                bookReview.Rating = createReviewVM.Rating;
                bookReview.Review = createReviewVM.Review;

                _db.Bookings.Update(bookReview);
                _db.SaveChanges();


                message = $"Success adding your new Review. ";
                    //+
                    //           $"Your new Review number is: {createReviewVM.BookingId}";
                success = true;

            }
            catch (Exception e)
            {
                message = $"Error creating your new Review, error: {e.Message}";
                success = false;

            }

            return Tuple.Create(createReviewVM.BookingId, message, success);
        }


        public IQueryable<SitterVM> GetTop3SitterVMs() { 

            var Top3Sitters = (from s in _db.Sitters.Include(s => s.Availabilities)
                               join u in _db.Users on s.UserId equals u.UserId
                               select new SitterVM
                               {
                                   SitterId = s.SitterId,
                                   FirstName = u.FirstName,
                                   Rate = (decimal)s.RatePerPetPerDay,
                                   ProfileBio = s.ProfileBio,
                                   ProfileImage = u.ProfileImage,
                                   AvgRating = (double)_db.Bookings.Where(b => b.SitterId == s.SitterId).Average(b => b.Rating),
                                   petTypes = _db.Sitters.Where(b => b.SitterId == s.SitterId).SelectMany(s => s.PetTypes).Select(p => p.PetType1).ToList(),
                                   availabilities = s.Availabilities.ToList(),
                               }).OrderByDescending(s => s.AvgRating)
                                 .ToList();

            if (Top3Sitters.Count > 3)
            {
                Top3Sitters = Top3Sitters.Take(3).ToList();
            }


            AvailabilityRepo availabilityRepo = new AvailabilityRepo(_db);
            foreach (var sitter in Top3Sitters)
            {
                if (sitter.availabilities != null)
                {
                    sitter.availableDates = availabilityRepo.GetAvailableDates(sitter.availabilities);
                }
            }

            return Top3Sitters.AsQueryable();


        }




        public RatingCountVM CountRating(int sitterId)
        {
            //SitterProfileVM sitter = GetSitterById(sitterId);

            var bookings = from b in _db.Bookings
                           where b.SitterId == sitterId
                           select new
                           {
                               b.Rating,
                           };


            RatingCountVM vm = new RatingCountVM();
            double total = 0;
            double one = 0;
            double two = 0;
            double three = 0;
            double four = 0;
            double five = 0;
            double reviews = (from b in _db.Bookings

                           where b.SitterId == sitterId && b.Review != null

                           select b.Review).Count();
            foreach (var b in bookings)
            {
                
                switch(b.Rating)
                {
                    case 5:
                        five++;
                        total++;
                        break;
                    case 4:
                        four++;
                        total++;
                        break;
                    case 3:
                        three++;
                        total++;
                        break;
                    case 2:
                        two++;
                        total++;
                        break;
                    case 1:
                        one++;
                        total++;
                        break;
                    default:
                        break;
                }
 
            }
            vm.Total = total;
            vm.Five = five;
            vm.Four = four;
            vm.Three = three;
            vm.Two = two;
            vm.One = one;

            return vm;

        }

    }
}
