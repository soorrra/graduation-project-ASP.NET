using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Petsitter.Models
{
    public partial class BookingNew
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        public decimal? Price { get; set; }
        public string? PaymentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SpecialRequests { get; set; }
        public int? Rating { get; set; }
        public string? Review { get; set; }
        public string? Complaint { get; set; }
        public int? SitterId { get; set; }
        public int? UserId { get; set; }

        public virtual Sitter? Sitter { get; set; }
        public virtual User? User { get; set; }

        public BookingNew(DateTime? startDate, DateTime? endDate, string? specialRequests,int? userId)
        {
            StartDate = startDate;
            EndDate = endDate;
            SpecialRequests = specialRequests;
            UserId = userId;
        }
    }
}
