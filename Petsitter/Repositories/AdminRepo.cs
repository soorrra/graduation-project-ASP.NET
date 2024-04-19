using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Petsitter.Repositories
{
    public class AdminRepo
    {
        private PetsitterContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private ApplicationDbContext _context;

        public AdminRepo(PetsitterContext context, IWebHostEnvironment webHost)
        {
            _db = context;
            webHostEnvironment = webHost;
          //  _context = context;


        }

        public IQueryable GetAllUsers()
        {

            var allUsers = (from user in _db.Users
                            select new AdminDashboardVM
                            {
                                UserID = user.UserId,
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Role = user.UserType
                            });

            return allUsers;


        }

        public void DeleteUserAndRelatedRecords(int userId)
        {
            // Retrieve the user entity
            var user = _db.Users.Include(u => u.Sitters).FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                // Delete related records from BookingPet table
                var bookingPets = _db.BookingPets.Where(bp => bp.Pet.UserId == userId);
                _db.BookingPets.RemoveRange(bookingPets);

                // Delete related records from Booking table
                var bookings = _db.Bookings.Where(b => b.Sitter.UserId == userId || b.UserId == userId);
                _db.Bookings.RemoveRange(bookings);

                //// Delete related records from SitterAvailability table
                //var sitterAvailabilities = _db.Availabilities.Where(sa => sa.Sitters.UserId == userId);
                //_db.Availabilities.RemoveRange(sitterAvailabilities);

                //// Delete related records from SitterPetType table
                //var sitterPetTypes = _db.PetTypes.Where(spt => spt.Sitters.UserId == userId);
                //_db.PetTypes.RemoveRange(sitterPetTypes);

                // Delete Sitter record if the user is a sitter
                var sitter = _db.Sitters.FirstOrDefault(s => s.UserId == userId);
                if (sitter != null)
                {
                    _db.Sitters.Remove(sitter);
                }

                // Delete the user
                _db.Users.Remove(user);

                // Save changes
                _db.SaveChanges();
            }
        }
    }
}

