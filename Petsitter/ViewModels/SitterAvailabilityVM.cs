using Petsitter.Models;
using System.ComponentModel;
using FoolProof.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Petsitter.ViewModels

{
    public class SitterAvailabilityVM
    {
        public int SitterId { get; set; }

        [DisplayName("Start Date")]
         public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        [GreaterThanOrEqualTo("StartDate")]
        public DateTime EndDate { get; set; }
        public List<Availability>? AvailableDates { get; set; }
        public List<Booking>? BookedDates { get; set; }
        public string? Message { get; set; }

    }
}
