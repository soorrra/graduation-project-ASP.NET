using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;
using System;

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
            //byte[] imageData = null;
            //if (newsVM.Image != null)
            //{
            //    using var binaryReader = new BinaryReader(newsVM.Image.OpenReadStream());
            //    imageData = binaryReader.ReadBytes((int)newsVM.Image.Length);
            //}

            News newsToAdd = new News
            {
                Title = newsVM.Title,
                Body = newsVM.Body,
                Date = DateTime.Now,
                //Image = imageData,
                Category = newsVM.Category
            };

            _db.News.Add(newsToAdd);
            _db.SaveChanges();
        }






    }
}

