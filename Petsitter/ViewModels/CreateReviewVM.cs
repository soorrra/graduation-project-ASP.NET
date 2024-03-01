using System.ComponentModel.DataAnnotations;

namespace Petsitter.ViewModels
{
    public class CreateReviewVM
    {
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }


        public string? PetParent { get; set; }

        public string? Sitter { get; set; }
        [Key]
        public int? SitterId { get; set; }
        // public int? UserId { get; set; }
        [Key]

        public int BookingId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public int Rating { get; set; }
        public string? Review { get; set; }
    }
}


