using Microsoft.AspNetCore.Hosting;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Drawing;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace Petsitter.Repositories
{
    public class PetRepo
    {
        PetsitterContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PetRepo(PetsitterContext context, IWebHostEnvironment webHost)
        {
            _db = context;
            webHostEnvironment = webHost;
        }

        public Tuple<int, string> CreatePetRecord(PetVM petVM, int userID)
        {
            Pet pet = new Pet();
            string message;
            //string stringFileName = UploadPetImageFile(petVM);

            try
            {
                if (petVM.PetImage != null && !IsImageFileTypeValid(petVM.PetImage.ContentType))
                {
                    message = "imageUpload" + "Please upload a PNG, " + "JPG, or JPEG file.";
                }
                else
                {
                    byte[] imageData = null;
                    if (petVM.PetImage != null)
                    {
                        using (var binaryReader = new BinaryReader(petVM.PetImage.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)petVM.PetImage.Length);
                        }
                    }

                    pet = new Pet
                    {
                        Name = petVM.Name,
                        BirthYear = petVM.BirthYear,
                        Sex = petVM.Sex,
                        PetSize = petVM.PetSize,
                        Instructions = petVM.Instructions,
                        UserId = userID,
                        PetType = petVM.PetType,
                        PetImage = imageData
                    };

                    _db.Pets.Add(pet);
                    _db.SaveChanges();

                    message = $"Success creating your new pet. " +
                                   $"Your new pet number is: {pet.PetId}";

                }
            }
            catch(Exception e)
            {
                pet.PetId = -1;
                message = $"Error creating your new pet, error: {e.Message}";

            }

            return Tuple.Create(pet.PetId, message);
        }

        public IEnumerable<Pet> GetPetLists(int id)
        {

            var pets = from p in _db.Pets where p.UserId == id select p;
            return pets;
        }


        public Pet GetPetDetailRecord(int petID, int userID)
        {
            var singlePet = _db.Pets.Where(p => p.PetId == petID).FirstOrDefault();

            Pet pet = new Pet
            {
                PetId = singlePet.PetId,
                Name = singlePet.Name,
                BirthYear = (int)singlePet.BirthYear,
                Sex = singlePet.Sex,
                PetSize = singlePet.PetSize,
                Instructions = singlePet.Instructions,
                UserId = userID,
                PetType = singlePet.PetType,
                PetImage = singlePet.PetImage
            };

            return pet;
        }

        public PetVM GetPetEditRecord(int petID, int userID)
        {
            var singlePet = _db.Pets.Where(p => p.PetId == petID).FirstOrDefault();

            PetVM vm = new PetVM
            {
                PetId = singlePet.PetId,
                Name = singlePet.Name,
                BirthYear = (int)singlePet.BirthYear,
                Sex = singlePet.Sex,
                PetSize = singlePet.PetSize,
                Instructions = singlePet.Instructions,
                UserId = userID,
                PetType = singlePet.PetType,
            };

            return vm;
        }


        public IEnumerable<Pet> GetPetData(int petID)
        {

            var pets = from p in _db.Pets where p.PetId == petID select p;
            return pets;
        }

        public Tuple<int, string> EditPet(PetVM petVM, int userID)
        {
            string updateMessage;
            byte[] imageData = null;
            //string stringFileName = UploadPetImageFile(petVM);

            Pet pet = new Pet
            {
                PetId = petVM.PetId,
                Name = petVM.Name,
                BirthYear = petVM.BirthYear,
                Sex = petVM.Sex,
                PetSize = petVM.PetSize,
                Instructions = petVM.Instructions,
                UserId = userID,
                PetType = petVM.PetType
            };

            if (petVM.PetImage != null)
            {
                if (!IsImageFileTypeValid(petVM.PetImage.ContentType))
                {
                    updateMessage = "imageUpload" + "Please upload a PNG, " + "JPG, or JPEG file.";
                    return Tuple.Create(pet.PetId, updateMessage);
                }

                using var binaryReader = new BinaryReader(petVM.PetImage.OpenReadStream());
                imageData = binaryReader.ReadBytes((int)petVM.PetImage.Length);
            }

            pet.PetImage = imageData;

            try
            {
                _db.Entry(pet).State = EntityState.Modified;

                if (pet.PetImage == null)
                {
                    _db.Entry(pet).Property(p => p.PetImage).IsModified = false;
                }

                _db.SaveChanges();

                updateMessage = $"Success editing {pet.Name} pet account " + $"Your edited pet number is: {pet.PetId}";
            }
            catch (Exception ex)
            {
                updateMessage = ex.Message;
            }

            return Tuple.Create(pet.PetId, updateMessage);
        }

        public string UploadPetImageFile(PetVM petVM)
        {
            string fileName = null;

            if (petVM.PetImage != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "_" + petVM.PetImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    petVM.PetImage.CopyTo(fileStream);
                }

            }

            return fileName;
        }

        public Tuple<string,int> DeletePetRecord(int petID)
        {
            string deleteMessage;

            var pet = _db.Pets.Where(p => p.PetId == petID).FirstOrDefault();

            try
            {
                _db.Remove(pet);
                _db.SaveChanges();

                deleteMessage = $"Success deleting {pet.Name} on your pet lists";
            }
            
            catch (Exception ex)
            {

                deleteMessage = ex.Message;
            }
            return Tuple.Create(deleteMessage, pet.PetId);
        }

        public bool IsBookedPet(int petID)
        {
            bool isBooked = false;
            var findBookedPet = _db.BookingPets.Where(b => b.PetId == petID).FirstOrDefault();

            if (findBookedPet != null)
            {
                return isBooked= true;
            }
            else
            {
                return isBooked = false;
            }
        }

        private bool IsImageFileTypeValid(string contentType)
        {
            return contentType == "image/png" ||
                   contentType == "image/jpeg" ||
                   contentType == "image/jpg";
        }

    }
}
