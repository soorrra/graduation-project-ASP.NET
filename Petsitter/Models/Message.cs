using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Petsitter.Models
{
    public partial class Message
    {
        [Key]
        public int messageID { get; set; }
        public int? chatID { get; set; }
        public int? fromUserID { get; set; }
        public int? toUserID { get; set; }
        public string? messageText { get; set; }
        public DateTime? timestamp { get; set; }

        public Chat Chat { get; set; } // Навигационное свойство для связи с таблицей Chats
        public User FromUser { get; set; } // Навигационное свойство для связи с таблицей Users для отправителя сообщения
        public User ToUser { get; set; } // Навигационное свойство для связи с таблицей Users для получателя сообщения
    }
}