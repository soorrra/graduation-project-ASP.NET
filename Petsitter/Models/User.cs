using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Petsitter.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Pets = new HashSet<Pet>();
            Sitters = new HashSet<Sitter>();
        }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? UserType { get; set; }
        public byte[]? ProfileImage { get; set; }
        public virtual UserType? UserTypeNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Sitter> Sitters { get; set; }
    }
}
