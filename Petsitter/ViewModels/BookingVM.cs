using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class BookingVM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BookingId { get; set; }
        public int SitterId { get; set; }
        [DisplayName("Sitter")]
        public string SitterName { get; set; }
        public int UserId { get; set; }
        public List<BookingPetVM> Pets { get; set; }

        [DisplayFormat(DataFormatString = "{0:D}")]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:D}")]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [DisplayName("Special Requests")]
        public string? SpecialRequests { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Payment Confirmation")]
        public string? PaymentId { get; set; }
    }
}
