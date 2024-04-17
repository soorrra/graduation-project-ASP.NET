using Microsoft.AspNetCore.Hosting;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Drawing;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace Petsitter.Repositories
{
    public class ChatRepo
    {
        private readonly PetsitterContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ChatRepo(PetsitterContext db)
        {
            _db = db;
        }

        public User getUserById(int id)
        {
            var user = (from s in _db.Users
                        where s.UserId == id
                        select s).FirstOrDefault();

            return user;
        } 
        public List<MessageVM> GetAllMess()
        {

            IQueryable<MessageVM> messages = from m in _db.Messages
                                             select new MessageVM
                                             {
                                                 messageID = m.messageID,
                                                 chatID = m.chatID,
                                                 fromUserID = m.fromUserID,
                                                 toUserID = m.toUserID,
                                                 messageText = m.messageText,
                                                 timestamp = (DateTime)m.timestamp
                                             };
            List<MessageVM> messageeList = messages.ToList();
            return messageeList;
        }

        public List<ChatVM> GetAllChats()
        {

            IQueryable<ChatVM> messages = from ch in _db.Chats
                                             select new ChatVM
                                             {
                                                chatID = ch.chatID,
                                                user1ID=ch.user1ID,
                                                user2ID=ch.user2ID
                                             };
            List<ChatVM> messageeList = messages.ToList();
            return messageeList;
        }

        public List<MessageVM> GetMessageVMByUserId(int id)
        {
            List<MessageVM> messages = GetAllMess();
            List<MessageVM> myMessages = new List<MessageVM>();
            foreach (var message in messages)
            {
                if (message.fromUserID == id || message.toUserID == id)
                {
                    myMessages.Add(message);
                }
            }

            return myMessages;
        }

        public List<ChatVM> GetChatVMByUserId(int id)
        {
            List<ChatVM> chats = GetAllChats();
            List<ChatVM> myChats = new List<ChatVM>();

            foreach (var chat in chats)
            {
                if (chat.user1ID == id || chat.user2ID == id)
                {
                    myChats.Add(chat);
                }
            }

            return myChats;

        }

        public IEnumerable<Chat> GetChatLists(int id)
        {
            var chats = _db.Chats
                           .Include(c => c.User1)
                           .Include(c => c.User2)
                           .Where(ch => ch.user1ID == id || ch.user2ID == id);
            return chats;
        }
    }
}