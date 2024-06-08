using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Petsitter.Models
{
    public partial class BookingConfirmation
    {
        [Key]
        public int BookingID { get; set; }
        public bool Confirmed { get; set; }
    }

}
