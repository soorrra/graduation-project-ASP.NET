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
        public string? Image { get; set; }
        public string? Category { get; set; }

        public static NewsVM FromModel(News news)
        {
            return new NewsVM
            {
                Id = news.Id,
                Title = news.Title,
                Body = news.Body,
                Date = news.Date,
                Image = news.Image,
                Category = news.Category
            };
        }
    }
}
