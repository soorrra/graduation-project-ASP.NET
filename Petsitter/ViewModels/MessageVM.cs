using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class MessageVM
    {
        [Key]
        public int messageID { get; set; }
        public int? chatID { get; set; }
        public int? fromUserID { get; set; }
        public int? toUserID { get; set; }
        public string? messageText { get; set; }
        public DateTime? timestamp { get; set; }
    }
}
