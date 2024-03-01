using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Petsitter.Models
{
    public partial class BookingPet
    {
        [Key]
        public int? BookingId { get; set; }
        [Key]
        public int? PetId { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Pet? Pet { get; set; }

        public BookingPet(int? bookingId, int? petId)
        {
            BookingId = bookingId;
            PetId = petId;
        }
    }
}
