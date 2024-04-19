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
        public Tuple<string, int> DeleteUserRecord(int userID)
        {
            string deleteMessage;

            var user = _db.Users.Where(p => p.UserId == userID).FirstOrDefault();

            try
            {
                _db.Remove(user);
                _db.SaveChanges();

                deleteMessage = $"Success deleting {user.FirstName} on your pet lists";
            }

            catch (Exception ex)
            {

                deleteMessage = ex.Message;
            }
            return Tuple.Create(deleteMessage, user.UserId);
        }

    }
}

