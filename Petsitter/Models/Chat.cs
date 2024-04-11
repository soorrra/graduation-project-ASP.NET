using System.ComponentModel;

namespace Petsitter.Models
{
    public class Chat
    {
        public int chatID { get; set; }
        public int? user1ID { get; set; }
        public int? user2ID { get; set; }


        public User User1 { get; set; } // Навигационное свойство для связи с таблицей Users для пользователя 1
        public User User2 { get; set; } // Навигационное свойство для связи с таблицей Users для пользователя 2
    }
}
