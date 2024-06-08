using Microsoft.AspNetCore.Mvc.Rendering;
using Petsitter.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class NewsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public IFormFile? Image { get; set; }
        public string? Category { get; set; }
    }
}


