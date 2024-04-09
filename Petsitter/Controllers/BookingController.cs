using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Petsitter.Data.Services;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;
using System.Globalization;

namespace Petsitter.Controllers
{
    public class BookingController : Controller
    {
        private readonly PetsitterContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public BookingController(PetsitterContext db, IEmailService emailService, IWebHostEnvironment webHost)
        {
            _db = db;
            _emailService = emailService;
            _webHostEnvironment = webHost;
        }

        [Authorize]
        public IActionResult ViewMyBookings()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            List<BookingVM> myBookings = bookingRepo.GetUpcomingBookingVMsByUserId(userId);

            return View(myBookings);
        }

        public IActionResult Chat(int sitterID)
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            // Получите пользователя из текущей сессии

            if (userId == null)
            {
                return RedirectToAction("Login"); // Перенаправление на страницу входа, если пользователь не аутентифицирован
            }

            CsFacingSitterRepo sitterRepo = new CsFacingSitterRepo(_db);
            SitterVM sitter = sitterRepo.GetSitterVM(sitterID);

            sitter.SitterId = sitterID;  // Метод для получения информации о ситтере по идентификатору

            if (sitter == null)
            {
                return RedirectToAction("SitterNotFound"); // Обработка случая, если ситтер не найден
            }

            ViewData["FirstName"] = sitter.FirstName;
            ViewData["SitterID"] = sitterID;

            return View("Chat");
        }

        [Authorize]
        public IActionResult ViewCreateBookingSitter()
        {
            BookingFormVM bookingg = new BookingFormVM();

            
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
           
            return View(bookingg);  
        }

        [Authorize]
        public IActionResult CreateBookAnnouncement(int bookingID)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            List<BookingVM> myBookings = bookingRepo.GetBookingVMsBySItterId();

            int bookingId = bookingRepo.CreateBookingAnnouncement(bookingID);

            return View(myBookings);

        }

        [Authorize]
        // POST: Initial Book
        [HttpPost]
        public IActionResult ViewCreateBookingSitter(BookingFormVM bbookingForm)
        {
            // If the message is null, set to an empty string.
            bbookingForm.Message ??= "";

            // Check that at least one pet was selected.
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
           

          
                        if (ModelState.IsValid)
                        {
                            int userId = 2;
                            int sitterId = 1;

                            // Create booking
                            int bookingId = bookingRepo.CreateBookBySitter(bbookingForm, sitterId ,userId);

                            // Redirect to confirmation and payment page
                            //return RedirectToAction("BookingDetails", "Booking", new {  bookingId });

    
                        }
                                   

            // Show booking page again.
            return View(bbookingForm);
        }

        [Authorize]
        public IActionResult ViewCreateBookingUser()
        {
            BookingFormVM bookingg = new BookingFormVM();

            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            List<BookingPetVM> pets = bookingRepo.GetBookingPetVMsByUserId(userId);
            bookingg.Pets = pets;

            return View(bookingg);
        }

        [Authorize]
        // POST: Initial Book
        [HttpPost]
        public IActionResult ViewCreateBookingUser(BookingFormVM bbookingForm)
        {
            // If the message is null, set to an empty string.
            bbookingForm.Message ??= "";

            // Check that at least one pet was selected.
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);



            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                // Create booking
                int bookingId = bookingRepo.CreateBookByUser(bbookingForm, userId);

                // Redirect to confirmation and payment page
                return RedirectToAction("BookingDetails", "Booking", new { bookingId });


            }


            // Show booking page again.
            return View(bbookingForm);
        }



        [Authorize]
        public IActionResult ViewAnnouncement()
        {
            //int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            //int sitterId = 1;

            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
           // List<BookingVM> myBookings = bookingRepo.GetBookingVMsByUserId(userId);

            List<BookingVM> myBookings = bookingRepo.GetBookingVMsBySItterId();
            //List<Booking> myBookings = bookingRepo.GetBookingsBySitter(sitterId);
            // List<BookingVM> bookings = bookingRepo.GetAllBookingVMs();

            return View(myBookings);
        }

        [Authorize]
        public IActionResult ViewPastBookings()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            List<BookingVM> myBookings = bookingRepo.GetPastBookingVMsByUserId(userId);

            return View(myBookings);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingID)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            BookingVM booking = bookingRepo.GetBookingVM(bookingID);

            // Check that the booking belongs to the currently logged in user
            if (booking.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID"))) {
                return View(booking);
            } else // If the booking is for a different user, redirect them to the no permission page.
            {
                return RedirectToAction("NoPermission", "Home");
            }
        }


        [Authorize]
        public IActionResult BookingDetailsAnnouncement(int bookingID)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            BookingVM booking = bookingRepo.GetBookingVM(bookingID);

            
                return View(booking);
           
        }

        public IActionResult FindASitter(int? page, List<string> petTypes, string selectedDates)
        {
            // Assign ViewBag values for customer's filter options.
            ViewBag.SelectedDates = selectedDates;
            ViewBag.SelectedPetTypes = petTypes;

            List<DateTime> dates = new List<DateTime>();
            if (selectedDates != null)
            {
                string[] selectedDatesString = selectedDates.Split(',');

                // Convert selected dates to DateTime objects.
                foreach (string date in selectedDatesString)
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(date.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        dates.Add(dt);
                    }
                }
            }

            // Get all pet types. 
            SitterRepos sitterRepos = new SitterRepos(_db, _webHostEnvironment);
            var allPetTypes = sitterRepos.getPetTypes();
            ViewBag.PetTypes = allPetTypes;

            // Get an IQueryable of all sitters.
            CsFacingSitterRepo sitterRepo = new CsFacingSitterRepo(_db);

            var allSitters = sitterRepo.GetAllSitterVMs().ToList();

            // Filter sitters.
            if ((petTypes != null && petTypes.Count > 0) && (dates != null && dates.Count > 0))
            {
                allSitters = allSitters.Where(s => s.petTypes.Any(pt => petTypes.Contains(pt)) && s.availableDates.Any(d => dates.Contains(d))).ToList();
            }
            else if (dates != null && dates.Count > 0)
            {
                allSitters = allSitters.Where(s => dates.All(d => s.availableDates.Contains(d))).ToList();
            }
            else if (petTypes != null && petTypes.Count > 0)
            {
                allSitters = allSitters.Where(s => s.petTypes.Any(pt => petTypes.Contains(pt))).ToList();
            }

            // Display 10 sitters per page.
            int pageSize = 10;

            return View(PaginatedList<SitterVM>.Create(allSitters.AsQueryable().AsNoTracking(), page ?? 1, pageSize));
        }
        
        //Book view

        [Authorize]
        // GET: Initial Book
        public IActionResult Book(int sitterID)
        {
            // Add sitter details to the booking form.
            BookingFormVM booking = new BookingFormVM();
            booking.SitterId = sitterID;
            CsFacingSitterRepo sitterRepo = new CsFacingSitterRepo(_db);
            var sitter = sitterRepo.GetSitterVM(sitterID);
            booking.SitterName = sitter.FirstName;

            // Get the current users pets to display on the form.
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            List<BookingPetVM> pets = bookingRepo.GetBookingPetVMsByUserId(userId);
            booking.Pets = pets;

            return View(booking);
        }


        [Authorize]
        // POST: Initial Book
        [HttpPost]
        public IActionResult Book(BookingFormVM bookingForm)
        {
            // If the message is null, set to an empty string.
            bookingForm.Message ??= "";

            // Check that at least one pet was selected.
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            bool petsSelected = bookingRepo.CheckPetSelection(bookingForm);

            // Check that sitter is available for selected dates.
            bool sitterAvailable = bookingRepo.CheckSitterAvailability(bookingForm);

            // Check that start date is after today's date.
            bool validDate = bookingRepo.CheckDate(bookingForm.StartDate);

            if (validDate)
            {
                if (sitterAvailable)
                {
                    if (petsSelected)
                    {
                        if (ModelState.IsValid)
                        {
                            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                            // Create booking
                            int bookingId = bookingRepo.Create(bookingForm, userId);

                            // Redirect to confirmation and payment page
                            return RedirectToAction("ConfirmBooking", "Booking", new { bookingId });
                        }
                    }
                    else // else if no pets have been selected
                    {
                        bookingForm.Message = "Please select at least one pet for this booking.";
                    }
                }
                else // else if sitter is not available
                {
                    bookingForm.Message = $"Sorry, {bookingForm.SitterName} is not available those days.";
                }
            } else // else if date is invalid 
            {
                bookingForm.Message = $"Please select a future date for the start date.";
            }

            // Show booking page again.
            return View(bookingForm);
        }
        //get available dates of sitter as events for booking form 
        public JsonResult GetEvents(int sitterID)
        {

            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            AvailabilityRepo availabilityRepo = new AvailabilityRepo(_db);

            var bookings = bookingRepo.GetBookingsBySitter(sitterID);
            var availabilities = availabilityRepo.GetAvailabilities(sitterID);

            var events = new List<object>();

            var bookedDates = bookingRepo.GetBookedDates(bookings);

            var availableDates = availabilityRepo.GetAvailableDates(availabilities);
            var notBooked = availableDates.Except(bookedDates);
            // Add not booked dates as events
            foreach (var date in notBooked)
            {
                if (date >= DateTime.Now)
                {
                    events.Add(new
                    {
                        title = "Available",
                        start = date.ToString("yyyy-MM-dd"),
                        display = "background",

                        color = "green"
                    });
                }
            }


            return Json(events);


        }

        [Authorize]
        public IActionResult ConfirmBooking(int bookingId)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            BookingVM confirmBooking = bookingRepo.GetBookingVM(bookingId);

            // Check that the booking belongs to the currently logged in user
            if (confirmBooking.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID")))
            {
                // Check that booking is not yet paid for 
                if (confirmBooking.PaymentId == null)
                {
                    return View(confirmBooking);
                }
                else // If booking has already been paid for, redirect to the booking details
                {
                    return RedirectToAction("BookingDetails", "Booking", new { bookingId = bookingId });
                }
            }
            else // If the booking is for a different user, redirect them to the no permission page.
            {
                return RedirectToAction("NoPermission", "Home");
            }
        }

        [Authorize]
        // GET: Edit
        public IActionResult Edit(int bookingId)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            BookingFormVM booking = bookingRepo.GetBookingFormVM(bookingId);
            BookingVM bookingDetails = bookingRepo.GetBookingVM(bookingId);

            // Check that the booking belongs to the currently logged in user
            if (bookingDetails.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID")))
            {
                return View(booking);
            }
            else // If the booking is for a different user, redirect them to the no permission page.
            {
                return RedirectToAction("NoPermission", "Home");
            }
        }

        [Authorize]
        // POST: Edit
        [HttpPost]
        public IActionResult Edit(BookingFormVM bookingForm)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);
            BookingVM bookingDetails = bookingRepo.GetBookingVM(bookingForm.BookingId);

            // Check that the booking belongs to the currently logged in user
            if (bookingDetails.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID")))
            {
                // If the message is null, set to an empty string.
                bookingForm.Message ??= "";

                // Check that at least one pet was selected.
                bool petsSelected = bookingRepo.CheckPetSelection(bookingForm);

                // Check that sitter is available for selected dates.
                bool sitterAvailable = bookingRepo.CheckSitterAvailability(bookingForm);

                if (sitterAvailable)
                {
                    if (petsSelected)
                    {
                        if (ModelState.IsValid)
                        {
                            // Update booking
                            int bookingId = bookingRepo.Update(bookingForm);

                            // Redirect to confirmation page
                            return RedirectToAction("ConfirmBooking", "Booking", new { bookingId = bookingId });
                        }
                    }
                    else // if no pets selected
                    {
                        bookingForm.Message = "Please select at least one pet for this booking.";
                    }
                }
                else // if sitter is not available
                {
                    bookingForm.Message = $"Sorry, {bookingForm.SitterName} is not available those days.";
                }

                // Show booking page again.
                return View(bookingForm);
            }
            else // If the booking is for a different user, redirect them to the no permission page.
            {
                return RedirectToAction("NoPermission", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult PaySuccess([FromBody] IPN ipn)
        {
            BookingRepo bookingRepo = new BookingRepo(_db, _emailService);

            string email = HttpContext.Session.GetString("Email");

            IPN completeIPN = bookingRepo.AddTransaction(ipn, email);

            return Json(completeIPN);
        }


        public IActionResult SitterDetails(int sitterID, string message, string? rating, int? page)
        {

            // Get the SitterVM.
            CsFacingSitterRepo cfsRepo = new CsFacingSitterRepo(_db);
            SitterVM sitterRes = cfsRepo.GetSitterVM(sitterID);
            //ViewData["Sitter"] = sitterRes;  

            if (message == null)
            {
                message = "";
            }
            ViewData["Confirm"] = message;



            User user = cfsRepo.getUserById(sitterID);
            ViewData["SitterProfileImg"] = user;

            ViewData["FirstName"] = sitterRes.FirstName;
            ViewData["AvgRating"] = sitterRes.AvgRating;
            ViewData["Rate"] = sitterRes.Rate.ToString("0.00");
            ViewData["ProfileBio"] = sitterRes.ProfileBio;
            ViewData["City"] = sitterRes.City;
            ViewBag.ProfileImage = sitterRes.ProfileImage;

            ReviewRepo rRepo = new ReviewRepo(_db);
            RatingCountVM sitterRating = rRepo.CountRating(sitterID);
            ViewData["TotalReview"] = sitterRating.Total;
            if (sitterRating.Total == 0 ||sitterRating == null)
            {
                ViewData["Star5"] = 0;
                ViewData["Star4"] = 0;
                ViewData["Star3"] = 0;
                ViewData["Star2"] = 0;
                ViewData["Star1"] = 0;
            }
            else { 
            ViewData["Star5"] = sitterRating.Five / sitterRating.Total * 100; ;
            ViewData["Star4"] = sitterRating.Four / sitterRating.Total * 100;
            ViewData["Star3"] = sitterRating.Three / sitterRating.Total * 100;
            ViewData["Star2"] = sitterRating.Two / sitterRating.Total * 100;
            ViewData["Star1"] = sitterRating.One / sitterRating.Total * 100;

            }
       

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //var rating 


            SitterRepos sitterReviews = new SitterRepos(_db, _webHostEnvironment);
            List<ReviewVM> response = sitterReviews.GetReviews(sitterID);
            //return View(response);
            if (!string.IsNullOrEmpty(rating) && rating != "-1")
            {
                 response = response.Where(r => r.rating == Int32.Parse(rating)).ToList();
                // Do something with the filteredResponse array

            }
            List<ReviewVM> responsecheck = response;

            ViewData["SitterID"] = sitterID;

            int pageSize = 6;
            return View(PaginatedList<ReviewVM>.Create(responsecheck.AsQueryable().AsNoTracking()
                                                     , page ?? 1, pageSize));
        }

        [Authorize]
        public IActionResult CreateReview(int bookingID)
        {
            // Get the booking information
            BookingRepo bRepo = new BookingRepo(_db, _emailService);
            var bookInfo = bRepo.GetBookingVM(bookingID);

            // Check that the booking belongs to the currently logged in user
            if (bookInfo.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID")))
            {
                // Get the sitter's information
                SitterRepos sRepos = new SitterRepos(_db, _webHostEnvironment);
                var sitterInfo = sRepos.GetSitterById(bookInfo.SitterId);

                // Get the sitter's profile image
                CsFacingSitterRepo cfsRepo = new CsFacingSitterRepo(_db);
                User user = cfsRepo.getUserById(bookInfo.SitterId);

                ViewData["SitterProfileImg"] = user;

                // Add sitter and booking details to the CreateReviewVM.
                CreateReviewVM reviewCreating = new CreateReviewVM
                {
                    Sitter = sitterInfo.FirstName + " " + sitterInfo.LastName,
                    SitterId = bookInfo.SitterId,
                    BookingId = bookingID,
                    StartDate = bookInfo.StartDate,
                    EndDate = bookInfo.EndDate,
                };

                ViewBag.SitterName = reviewCreating.Sitter;

                return View(reviewCreating);
            }
            else // If the booking is for a different user, redirect them to the no permission page.
            {
                return RedirectToAction("NoPermission", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateReview(CreateReviewVM createReviewVM)
        {
            // Get the booking information
            BookingRepo bRepo = new BookingRepo(_db, _emailService );
            var bookInfo = bRepo.GetBookingVM(createReviewVM.BookingId);

            // Check that the booking belongs to the currently logged in user
            if (bookInfo.UserId == Convert.ToInt32(HttpContext.Session.GetString("UserID")))
            {
                ReviewRepo reviewRepo = new ReviewRepo(_db);

                Tuple<int, string, bool> response =
                    reviewRepo.UpdateReview(createReviewVM);

                //return RedirectToAction("SitterDetails", "Booking", new { createReviewVM.SitterId });

                if (response.Item3) // Check if the operation was successful
                {
                    return RedirectToAction("SitterDetails", "Booking", new { createReviewVM.SitterId, message = response.Item2});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create review");
                    return View("CreateReview", createReviewVM);
                }

            } else // If the booking is for a different user, redirect them to the no permission page.
            {
                return RedirectToAction("NoPermission", "Home");
            }
        }
    }
}
