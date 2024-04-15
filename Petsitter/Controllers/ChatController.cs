using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Petsitter.Data.Services;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;
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

        [Authorize]
        public IActionResult ViewMyChats()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            List<BookingVM> myBookings = bookingRepo.GetUpcomingBookingVMsByUserId(userId);

            return View(myBookings);
        }

        public IActionResult Chat(int sitterID)
        {
            ChatRepo chatRepo = new ChatRepo(_db);
            //сомнительная хрень, все х переделывай 
            string userName = HttpContext.Session.GetString("UserName");
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int fromUserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            List<MessageVM> myMessages = chatRepo.GetMessageVMByUserId(fromUserID);


            if (userId == null)
            {
                return RedirectToPage("./Register");

            }
            //


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
    }
}
