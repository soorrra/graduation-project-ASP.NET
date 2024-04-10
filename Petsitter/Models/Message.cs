using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Petsitter.Models
{
    public partial class PetType
    {
        [Key]
        public int chatID { get; set; }
        [Key]
        public int fromUserID { get; set; }
        [Key]
        public int toUserID { get; set; }
        public string messageText { get; set; }
        public DateTime timestamp { get; set; }
    }
}