using FoolProof.Core;
using Microsoft.AspNetCore.Mvc;
using Petsitter.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
        public class CreateBookingFormVM
        {
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int BookingId { get; set; }

            [BindProperty]
            public List<BookingPetVM> Pets { get; set; } = new List<BookingPetVM>();
            [DisplayName("Start Date")]
            public DateTime StartDate { get; set; }
            [DisplayName("End Date")]
            [GreaterThanOrEqualTo("StartDate")]
            public DateTime EndDate { get; set; }
            [DisplayName("Special Requests")]
            public string? SpecialRequests { get; set; }
            public string? Message { get; set; }

        }

}
