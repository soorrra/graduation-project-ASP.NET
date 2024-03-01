using System.ComponentModel.DataAnnotations;

namespace Petsitter.ViewModels
{
    public class ReviewVM
    {
        public string? petParent { get; set; }
        public string? sitter { get; set; }
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime? startDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime? endDate { get; set; }
        public int? rating { get; set; }
        public string? review { get; set; }
        public byte[]? profileImage { get; set; }
    }
}


