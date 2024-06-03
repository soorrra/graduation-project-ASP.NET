using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Cms;
using Petsitter.Data.Services;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;
using System.Drawing.Printing;
using System.Globalization;

namespace Petsitter.Controllers
{
    public class ChatController : Controller
    {
        private readonly PetsitterContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public ChatController(PetsitterContext db, IEmailService emailService, IWebHostEnvironment webHost)
        {
            _db = db;
            _emailService = emailService;
            _webHostEnvironment = webHost;
        }

        
        public IActionResult Chat(int sitterID)
        {
            ChatRepo chatRepo = new ChatRepo(_db);
            string userName = HttpContext.Session.GetString("UserName");
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int fromUserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            List<MessageVM> myMessages = chatRepo.GetMessageVMByUserId(fromUserID);


            if (userId == null)
            {
                return RedirectToPage("./Register");

            }
            

            CsFacingSitterRepo sitterRepo = new CsFacingSitterRepo(_db);
            SitterVM sitter = sitterRepo.GetSitterVM(sitterID);
            int toUserId = sitterRepo.getUserIdBySitterId(sitterID);

            sitter.SitterId = sitterID;

            if (sitter == null)
            {
                return RedirectToAction("SitterNotFound");
            }

            ViewData["FirstName"] = sitter.FirstName;
            ViewData["fromUserID"] = fromUserID;
            ViewData["toUserID"] = toUserId;
            ViewData["Messages"] = myMessages;
            ViewData["UserName"] = userName;


            return View(myMessages);
        }

        public IActionResult ChatFromMyChats(int userID)
        {
            ChatRepo chatRepo = new ChatRepo(_db);
            //сомнительная хрень, все х переделывай 
            string userName = HttpContext.Session.GetString("UserName");
            int fromUserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            List<MessageVM> myMessages = chatRepo.GetMessageVMByUserId(fromUserID);


            int toUserId = userID;
            User toUser = chatRepo.getUserById(userID);
            string toFirstName= toUser.FirstName;

            ViewData["FirstName"] = toFirstName;
            ViewData["fromUserID"] = fromUserID;
            ViewData["toUserID"] = toUserId;
            ViewData["Messages"] = myMessages;
            ViewData["UserName"] = userName;


            return View("Chat", myMessages);
        }


        [Authorize]
        public IActionResult ViewMyChats()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            ChatRepo chatRepo = new ChatRepo(_db);

            List<MessageVM> myMessages = chatRepo.GetMessageVMByUserId(userId);
            var lastMessage = myMessages.LastOrDefault();

          


            //List<ChatVM> myChats = chatRepo.GetChatVMByUserId(userId);
            ViewData["ChatLists"] = chatRepo.GetChatLists(userId);
            ViewData["CurrUser"] = userId;



            //ViewData["lastMessage"] = lastMessage;
            //ViewData["UserName"] = userName;
            //ViewData["ProfileImage"] = ProfileImage;

            return View("ViewMyChats");
        }


    }
}
