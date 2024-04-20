using NuGet.Protocol;
using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public partial class Sitter
    {
        public Sitter()
        {
            Bookings = new HashSet<Booking>();
            Availabilities = new HashSet<Availability>();
            PetTypes = new HashSet<PetType>();
            ServiceTypes = new HashSet<ServiceType>();

        }

        public int SitterId { get; set; }
        public decimal? RatePerPetPerDay { get; set; }
        public string? ProfileBio { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<PetType> PetTypes { get; set; }
        public virtual ICollection<ServiceType> ServiceTypes { get; set; }

    }
}
