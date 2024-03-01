using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;

namespace Petsitter.Repositories
{
    public class CustomerRepo
    {
        PetsitterContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CustomerRepo(PetsitterContext context, IWebHostEnvironment webHost)
        {
            _db = context;
            webHostEnvironment = webHost;
        }

        public void AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User GetCustomerId(string email)
        {
            var customers = _db.Users.Where(u => u.Email == email).FirstOrDefault();

            return customers;
        }

        public CustomerVM GetProfile(int customerID)
        {
            var singleUser = _db.Users.Where(u => u.UserId == customerID).FirstOrDefault();

            var petLists = _db.Pets.Where(p => p.UserId == customerID).ToList();

            // find booking pets
            var findBookedPets = new List<BookingPet>();

            // find booking pet dates
            var findBookingDates = new List<Booking>();

            foreach (var pet in petLists)
            {
                var bookedPets = _db.BookingPets.Where(b => b.PetId == pet.PetId).ToList();

                // process of find a booked pet from Booking table
                var findABookedPet = _db.BookingPets.Where(b => b.PetId == pet.PetId).FirstOrDefault();
                var findBookingDate = _db.Bookings.Where(b => findABookedPet != null && b.BookingId == findABookedPet.BookingId).ToList();

                if (findABookedPet != null)
                {
                    findBookingDates.AddRange(findBookingDate);
                }
                findBookedPets.AddRange(bookedPets);
            }

            CustomerVM vm = new CustomerVM
            {
                CustomerId = customerID,
                FirstName = singleUser.FirstName,
                LastName = singleUser.LastName,
                Email = singleUser.Email,
                PostalCode = singleUser.PostalCode,
                PhoneNumber = singleUser.PhoneNumber,
                StreetAddress = singleUser.StreetAddress,
                City = singleUser.City,
                UserType = singleUser.UserType,
                BookedPets = findBookedPets,
                Booking = findBookingDates
            };

            return vm;
        }

        public IEnumerable<User> GetUserData(int id)
        {

            var users = from u in _db.Users where u.UserId == id select u;
            return users;
        }

        public Tuple<int, string> EditProfile(CustomerVM customerVM, int userID)
        {
            string updateMessage;
            User user = new User();
            //string stringFileName = UploadCustomerFile(customerVM);

            try
            {
                if (customerVM.ProfileImage != null && !IsImageFileTypeValid(customerVM.ProfileImage.ContentType))
                {
                    updateMessage = "imageUpload" + "Please upload a PNG, " + "JPG, or JPEG file.";
                }
                else
                {
                    byte[] imageData = null;
                    if (customerVM.ProfileImage != null)
                    {
                        using var binaryReader = new BinaryReader(customerVM.ProfileImage.OpenReadStream());
                        imageData = binaryReader.ReadBytes((int)customerVM.ProfileImage.Length);
                    }

                    user = new User
                    {
                        UserId = userID,
                        FirstName = customerVM.FirstName,
                        LastName = customerVM.LastName,
                        Email = customerVM.Email,
                        PostalCode = customerVM.PostalCode,
                        PhoneNumber = customerVM.PhoneNumber,
                        StreetAddress = customerVM.StreetAddress,
                        City = customerVM.City,
                        UserType = customerVM.UserType,
                        ProfileImage = imageData
                    };

                    try
                    {
                        _db.Entry(user).State = EntityState.Modified;
                        if (user.ProfileImage == null)
                        {
                            _db.Entry(user).Property(u => u.ProfileImage).IsModified = false;
                        }
                        _db.SaveChanges();

                        updateMessage = $"Success editing {user.FirstName} user account " + $"Your edited user number is: {user.UserId}";
                    }
                    catch (Exception ex)
                    {
                        updateMessage = ex.Message;
                    }
                }
                return Tuple.Create(user.UserId, updateMessage);
            }
            catch (Exception ex)
            {
                updateMessage = ex.Message;
                return Tuple.Create(userID, updateMessage);
            }
        }

        private string UploadCustomerFile(CustomerVM customerVM)
        {
            string fileName = null;
            if (customerVM.ProfileImage != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "_" + customerVM.ProfileImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    customerVM.ProfileImage.CopyTo(fileStream);
                }
            } 
            return fileName;
        }


        private bool IsImageFileTypeValid(string contentType)
        {
            return contentType == "image/png" ||
                   contentType == "image/jpeg" ||
                   contentType == "image/jpg";
        }

    }
}

