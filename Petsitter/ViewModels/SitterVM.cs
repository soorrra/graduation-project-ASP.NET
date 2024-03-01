using Petsitter.Models;
using System.ComponentModel;
using System.Security.Policy;

namespace Petsitter.ViewModels
{
    public class SitterVM
    {
        public int SitterId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Rate per Day per Pet")]
        public decimal Rate { get; set; }

        [DisplayName("About")]
        public string ProfileBio { get; set; }

        public byte[]? ProfileImage { get; set; }

        [DisplayName("Average Rating")]
        public double? AvgRating { get; set; }

        public string? City { get; set; }
        public List<ReviewVM>? Reviews { get; set; }
        public List<string>? petTypes { get; set; }
        public List<DateTime>? availableDates { get; set; }
        public List<Availability>? availabilities  { get; set; }
    }
}
