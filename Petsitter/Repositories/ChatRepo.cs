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
        public List<MessageVM> GetMessageVMByUserId(int id)
        {
            List<MessageVM> messages = GetAllMess();
            List<MessageVM> myMessages = new List<MessageVM>();
            foreach (var message in messages)
            {
                if (message.fromUserID == id|| message.toUserID == id)
                {
                    myMessages.Add(message);
                }
            }
            
            return myMessages;
        }

    }
}
