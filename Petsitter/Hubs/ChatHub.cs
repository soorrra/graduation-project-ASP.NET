using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Petsitter.Models;
using System;
using System.Threading.Tasks;

namespace Petsitter.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly PetsitterContext _db;

        public ChatHub(PetsitterContext db)
        {
            _db = db;
        }

        public async Task SendMessage( string message, int fromUserID, int toUserID)
        {
            var userName = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", userName, message);

            var chatId = GetOrCreateChatId(fromUserID, toUserID);

            // Сохранение сообщения в базу данных
            var newMessage = new Message
            {
                chatID = chatId, // Здесь нужно указать соответствующий chatID
                fromUserID = fromUserID, // Здесь нужно указать соответствующий fromUserID
                toUserID = toUserID, // Здесь нужно указать соответствующий toUserID
                messageText = message,
                timestamp = DateTime.UtcNow
            };

            _db.Messages.Add(newMessage);
            await _db.SaveChangesAsync();


        }

        private int GetOrCreateChatId(int user1Id, int user2Id)
        {
            // Проверка существующего чата между пользователями
            var existingChat = _db.Chats.FirstOrDefault(c =>
                (c.user1ID == user1Id && c.user2ID == user2Id) ||
                (c.user1ID == user2Id && c.user2ID == user1Id));

            if (existingChat != null)
            {
                return existingChat.chatID;
            }
            else
            {
                // Создание нового чата
                var newChat = new Chat
                {
                    user1ID = user1Id,
                    user2ID = user2Id
                };

                _db.Chats.Add(newChat);
                _db.SaveChanges();

                return newChat.chatID;
            }
        }

        public async Task GetMessages(int fromUserID, int toUserID)
        {
            var messages = _db.Messages
                .Where(m => (m.fromUserID == fromUserID && m.toUserID == toUserID) || (m.fromUserID == toUserID && m.toUserID == fromUserID))
                .OrderBy(m => m.timestamp)
                .ToList();

            await Clients.Caller.SendAsync("LoadMessages", messages);
        }

    }
}
