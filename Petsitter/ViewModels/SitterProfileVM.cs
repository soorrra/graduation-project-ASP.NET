using Petsitter.Data;
using Petsitter.Models;
using Petsitter.Repositories;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class SitterProfileVM
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? UserType { get; set; }
        public int SitterId { get; set; }
        public decimal? RatePerPetPerDay { get; set; }
        public string? ProfileBio { get; set; }
        [NotMapped]
        public IFormFile? ProfileImage { get; set; }
        public List<string>? PetTypesAvailable { get; set; }
        public List<string>? SelectedPetTypes { get; set; }
        public List<string>? ServiceTypesAvailable { get; set; }
        public List<string>? SelectedServiceTypes { get; set; }
        public string? Message { get; set; }
        public double? AvgRating { get; set; }

    }
}
