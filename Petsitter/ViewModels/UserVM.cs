using System.ComponentModel.DataAnnotations;

namespace Petsitter.ViewModels
{
    public class UserVM
    {
        // ASP.NET USER
        [Key]
        [Required]
        public string Email { get; set; }

    }
}