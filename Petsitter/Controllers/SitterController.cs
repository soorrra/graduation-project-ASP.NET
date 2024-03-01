using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;
using SendGrid;
using System.Collections.Generic;
using System;
using System.Drawing.Drawing2D;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Petsitter.Data.Services;


namespace Petsitter.Controllers
{
    [Authorize]

    public class SitterController : Controller
    {
        private readonly PetsitterContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CustomerController> _logger;
       private readonly IEmailService _emailService;


        public static int clientID;
        public SitterController(ILogger<CustomerController> logger, PetsitterContext db, IWebHostEnvironment webHost, IEmailService emailService)
        {
            _db = db;
            _logger = logger;
            _webHostEnvironment = webHost;
           _emailService = emailService;

        }
        /// <summary>
        /// get  bookings  list 
        /// </summary>
        /// <returns></returns>
        public IActionResult Dashboard(int? page, string status)
        {
            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));

            SitterRepos sitterRepos = new SitterRepos(_db, _webHostEnvironment);
            IEnumerable<SitterDashboardVM> bookings = sitterRepos.GetBooking(sitterID); ;
            ViewData["UpComing"] = bookings.Select(b => b.upComingNbr).LastOrDefault();
            ViewData["Complete"] = bookings.Select(b => b.completeNbr).LastOrDefault();
            ViewData["Reviews"] = bookings.Select(b => b.reviewsNbr).LastOrDefault();
            if (!string.IsNullOrEmpty(status))
            {

                bookings = sitterRepos.GetBookingByStatus(bookings, status);



            }


            int pageSize = 4;

            return View(PaginatedList<SitterDashboardVM>.Create(bookings.AsQueryable().AsNoTracking(), page ?? 1, pageSize));

        }
        /// <summary>
        /// get details of booking with pet parent informatins
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));

            SitterRepos sitterRepos = new SitterRepos(_db, _webHostEnvironment);
            SitterProfileVM sitter = sitterRepos.GetSitterById(sitterID);
            SitterDashboardVM booking = sitterRepos.GetBookingDetails(id);
            //var geocoder = new Yandex.Geocode(YourAPIKey);
            //string customerAddress = $"{booking.user.StreetAddress}, {booking.user.City}, {booking.user.PostalCode}";
            //string sitterAddress = $"{sitter.StreetAddress}, {sitter.City}, {sitter.PostalCode}";

            //booking.PointCustomer = locationService.GetLatLongFromAddress(customerAddress);
            //booking.PointSitter = locationService.GetLatLongFromAddress(sitterAddress);

            return View(booking);
        }
        
        /// <summary>
        /// get all sitter information 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IActionResult Profile(string message)
        {
            SitterRepos sitterRepos = new SitterRepos(_db, _webHostEnvironment);
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            int userID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            ViewData["UserProfileImg"] = sitterRepos.getUser(userID);

            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));

            SitterProfileVM sitterProfileVM = sitterRepos.GetSitterById(sitterID);

            sitterProfileVM.Message = message;
            var defaultImageFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/default-image.jpg");
            byte[] defaultImageBytes;
            using (var fileStream = new FileStream(defaultImageFilePath, FileMode.Open))
            {
                using var binaryReader = new BinaryReader(fileStream);
                defaultImageBytes = binaryReader.ReadBytes((int)fileStream.Length);
            }
            ViewData["ProfileImg"] = defaultImageBytes;


            return View(sitterProfileVM);
        }
        /// <summary>
        /// edit sitter profile
        /// </summary>
        /// <returns></returns>
        public IActionResult EditProfile()
        {
            SitterRepos sitterRepos = new SitterRepos(_db, _webHostEnvironment);
            int userID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));

            SitterProfileVM vm = sitterRepos.GetSitterById(sitterID);

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            return View(vm);
        }
        [HttpPost]
        public IActionResult EditProfile(SitterProfileVM sitterProfileVM)
        {
            sitterProfileVM.Message = "Invalid entry please try again";

            SitterRepos sitterRepos = new SitterRepos(_db, _webHostEnvironment);


            if (ModelState.IsValid)
            {

                Tuple<int, string> response = sitterRepos.EditSitter(sitterProfileVM);

                if (response.Item1 < 0)
                {
                    sitterProfileVM.Message = response.Item2;
                }
                else
                {
                    return RedirectToAction("Profile", "Sitter", new { message = response.Item2 });
                }
            }

            return View(sitterProfileVM);
        }
        /// <summary>
        /// add new availability and view all booked and not booked day 
        /// </summary>
        /// <returns></returns>
        public IActionResult Availability()
        {
            // Get the logged in sitter ID
            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));

            SitterAvailabilityVM vm = new SitterAvailabilityVM();
            // Set the sitter ID for the view model
            vm.SitterId = sitterID;
            return View(vm);


        }


        [HttpPost]
        public IActionResult Availability(SitterAvailabilityVM availability)
        {
            availability.Message = "Invalid entry please try again";

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                AvailabilityRepo availabilityRepo = new AvailabilityRepo(_db);
                Tuple<int, string> response = availabilityRepo.AddAvailability(availability);
                if (response.Item1 <= 0)
                {
                    availability.Message = response.Item2;
                }
                else
                {
                    return RedirectToAction("Availability", "Sitter");
                }
            }

            // If the model is not valid, return the view with the errors
            return View(availability);

        }
        /// <summary>
        /// get the booked and not booked as json result
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEvents()
        {
            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));

            BookingRepo bookingRepo = new BookingRepo(_db , _emailService);
            AvailabilityRepo availabilityRepo = new AvailabilityRepo(_db);

            var bookings = bookingRepo.GetBookingsBySitter(sitterID);
            var availabilities = availabilityRepo.GetAvailabilities(sitterID);

            var events = new List<object>();

            var bookedDates = bookingRepo.GetBookedDates(bookings);
            //Add booked dates as events

            foreach (var date in bookedDates)
            {
                if (date >= DateTime.Now)
                {
                    events.Add(new
                    {
                        title = "booked",
                        start = date.ToString("yyyy-MM-dd"),
                        display = "background",
                        color = "red"
                    });
                }
            }

            var availableDates = availabilityRepo.GetAvailableDates(availabilities);
            var notBooked = availableDates.Except(bookedDates);
            // Add not booked dates as events
            foreach (var date in notBooked)
            {
                if (date >= DateTime.Now)
                {
                    events.Add(new
                    {
                        title = "not booked",
                        start = date.ToString("yyyy-MM-dd"),
                        display = "background",

                        color = "green"
                    });
                }
            }


            return Json(events);


        }


        public IActionResult ReviewList(string? rating, int? page)
        {
            int sitterID = Convert.ToInt32(HttpContext.Session.GetString("SitterID"));


            CsFacingSitterRepo cfsRepo = new CsFacingSitterRepo(_db);
            SitterVM sitterRes = cfsRepo.GetSitterVM(sitterID);

            User user = cfsRepo.getUserById(sitterID);
            ViewData["SitterProfileImg"] = user;

            // Get the logged in sitter ID
            ViewData["FirstName"] = sitterRes.FirstName;
            ViewData["AvgRating"] = sitterRes.AvgRating;
            ViewData["Rate"] = sitterRes.Rate.ToString("0.00");
            ViewData["ProfileBio"] = sitterRes.ProfileBio;
            ViewBag.ProfileImage = sitterRes.ProfileImage;

            //var rating 


            ReviewRepo rRepo = new ReviewRepo(_db);
            RatingCountVM sitterRating = rRepo.CountRating(sitterID);
            ViewData["TotalReview"] = sitterRating.Total;
            if (sitterRating.Total == 0 || sitterRating == null)
            {
                ViewData["Star5"] = 0;
                ViewData["Star4"] = 0;
                ViewData["Star3"] = 0;
                ViewData["Star2"] = 0;
                ViewData["Star1"] = 0;
            }
            else
            {
                ViewData["Star5"] = sitterRating.Five / sitterRating.Total * 100; ;
                ViewData["Star4"] = sitterRating.Four / sitterRating.Total * 100;
                ViewData["Star3"] = sitterRating.Three / sitterRating.Total * 100;
                ViewData["Star2"] = sitterRating.Two / sitterRating.Total * 100;
                ViewData["Star1"] = sitterRating.One / sitterRating.Total * 100;

            }


            SitterRepos sitterReviews = new SitterRepos(_db, _webHostEnvironment);


            List<ReviewVM> response = sitterReviews.GetReviews(sitterID);
            //return View(response);

            //if (rating != null)
            if (!string.IsNullOrEmpty(rating) && rating != "-1")
            {
                response = response.Where(r => r.rating == Int32.Parse(rating)).ToList();
                // Do something with the filteredResponse array

            }
            List<ReviewVM> responsecheck = response;

            ViewData["SitterID"] = sitterID;

            int pageSize = 9;

            return View(PaginatedList<ReviewVM>.Create(responsecheck.AsQueryable().AsNoTracking()
                                                     , page ?? 1, pageSize));

        }

    }






}


