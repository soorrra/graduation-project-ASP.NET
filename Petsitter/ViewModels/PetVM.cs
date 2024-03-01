using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class PetVM
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public string Sex { get; set; }
        public string PetSize { get; set; }
        public string Instructions { get; set; }
        public int UserId { get; set; }
        public string PetType { get; set; }

        public IFormFile? PetImage { get; set; }

        public List<string> AvailableSizes { get; set; }

    }
}
