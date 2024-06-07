using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }
    }
}
