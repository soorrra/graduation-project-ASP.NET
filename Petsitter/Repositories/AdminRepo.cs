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

            var user = _db.Users.Include(u => u.Sitters).FirstOrDefault(p => p.UserId == userID);

            if (user != null)
            {
                try
                {
                    // Удаляем пользователя и все связанные с ним записи в таблице Sitter
                    _db.Users.Remove(user);
                    _db.SaveChanges();

                    deleteMessage = $"Success deleting {user.FirstName} on your pet lists";
                }
                catch (Exception ex)
                {
                    deleteMessage = ex.Message;
                }
            }
            else
            {
                deleteMessage = $"User with ID {userID} not found";
            }

            return Tuple.Create(deleteMessage, userID);
        }

        public void AddPetType(PetType petType)
        {
            _db.PetTypes.Add(petType);
            _db.SaveChanges();
        }

        public void AddNews(NewsVM newsVM)
        {
            News newsToAdd = new News
            {
                Title = newsVM.Title,
                Body = newsVM.Body,
                Date = newsVM.Date,
                Image = newsVM.Image,
                Category = newsVM.Category
            };

            _db.News.Add(newsToAdd);
            _db.SaveChanges();
        }
    }
}

