using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public partial class Pet
    {
        public int PetId { get; set; }
        public string? Name { get; set; }
        public int? BirthYear { get; set; }
        public string? Sex { get; set; }
        public string? PetSize { get; set; }
        public string? Instructions { get; set; }
        public int? UserId { get; set; }
        public string? PetType { get; set; }
        public byte[]? PetImage { get; set; }
        public virtual PetType? PetTypeNavigation { get; set; }
        public virtual User? User { get; set; }
    }
}
