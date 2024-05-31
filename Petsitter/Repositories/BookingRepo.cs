using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Petsitter.Data.Services;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;

namespace Petsitter.Repositories
{
    public class BookingRepo
    {
        private readonly PetsitterContext _db;
        private readonly IEmailService _emailService;

        public BookingRepo(PetsitterContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        // SECTION: Read Methods

        public Booking GetBooking(int bookingId)
        {
            return _db.Bookings.Where(b => b.BookingId == bookingId).FirstOrDefault();
        }

        public List<BookingVM> GetAllBookingVMs()
        {
            IQueryable<BookingVM> bookings = from b in _db.Bookings                                            
                                             select new BookingVM
                                             {
                                                 BookingId = b.BookingId,
                                                 SitterId = (int)b.SitterId,
                                                 UserId = (int)b.UserId,
                                                 StartDate = (DateTime)b.StartDate,
                                                 EndDate = (DateTime)b.EndDate,
                                                 SpecialRequests = b.SpecialRequests,
                                                 Price = (decimal)b.Price,
                                                 PaymentId = b.PaymentId
                                             };

            // Convert to list so that it can be looped through to add pets.
            List<BookingVM> bookingsList = bookings.ToList();

            // Get all BookingPetVMs.
            IQueryable<BookingPetVM> bookingPetVMs = from p in _db.Pets
                                                     select new BookingPetVM
                                                     {
                                                         PetId = p.PetId,
                                                         Name = p.Name
                                                     };

            // Assign the appropriate BookingPetVM to each BookingVM, along with Sitter names.
            foreach (var booking in bookingsList)
            {
                List<int> petIds = _db.BookingPets.Where(bp => bp.BookingId == booking.BookingId).Where(bp => bp.PetId.HasValue).Select(bp => bp.PetId.GetValueOrDefault()).ToList();

                List<BookingPetVM> pets = new List<BookingPetVM>();
                foreach (var petId in petIds)
                {
                    pets.Add(bookingPetVMs.Where(pv => pv.PetId == petId).FirstOrDefault());
                }

                booking.Pets = pets;

                // Add sitter name.
                Sitter sitter = _db.Sitters.Where(s => s.SitterId == booking.SitterId).FirstOrDefault();
                User user = _db.Users.Where(u => u.UserId == sitter.UserId).FirstOrDefault();
                booking.SitterName = user.FirstName;
            }

            return bookingsList;
        }
        public BookingVM GetBookingVM(int bookingID)
        {
            return GetAllBookingVMs().Where(b => b.BookingId == bookingID).FirstOrDefault();
        }

        public List<BookingVM> GetBookingVMsByUserId(int userID)
        {
            List<BookingVM> bookings = GetAllBookingVMs();
            List<BookingVM> myBookings = new List<BookingVM>();

            // Get only confirmed and paid for bookings.
            foreach (var booking in bookings)
            {
                if(booking.UserId == userID /*&& booking.PaymentId != null*/)
                {
                    myBookings.Add(booking);
                }
            }
            return myBookings;
        }

        public List<BookingVM> GetBookingVMsBySItterId()
        {
            List<BookingVM> bookings = GetAllBookingVMs();
            List<BookingVM> myBookings = new List<BookingVM>();

            // Get only confirmed and paid for bookings.
            foreach (var booking in bookings)
            {
                if (booking.SitterId == 1 /*&& booking.PaymentId != null*/)
                {
                    myBookings.Add(booking);
                }
            }
            return myBookings;
        }


        public List<BookingVM> GetUpcomingBookingVMsByUserId(int userID)
        {
            List<BookingVM> myBookings = GetBookingVMsByUserId(userID);
            List<BookingVM> upcomingBookings = new List<BookingVM>();

            // Filter for upcoming bookings only
            foreach (var booking in myBookings)
            {
                if (booking.EndDate > DateTime.Now)
                {
                    upcomingBookings.Add(booking);
                }
            }

            // Sort soonest to furthest away
            upcomingBookings = upcomingBookings.OrderBy(b => b.StartDate).ToList();

            return upcomingBookings;
        }

        public List<BookingVM> GetBookingsStartingTomorrow()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);
            IQueryable<BookingVM> bookings = from b in _db.Bookings
                                             where b.StartDate == tomorrow
                                             select new BookingVM
                                             {
                                                 BookingId = b.BookingId,
                                                 SitterId = (int)b.SitterId,
                                                 UserId = (int)b.UserId,
                                                 StartDate = (DateTime)b.StartDate,
                                                 EndDate = (DateTime)b.EndDate,
                                                 SpecialRequests = b.SpecialRequests,
                                                 Price = (decimal)b.Price,
                                                 PaymentId = b.PaymentId
                                             };


            // Convert to list so that it can be looped through to add pets.
            List<BookingVM> bookingsTomarrowList = bookings.ToList();

            // Get all BookingPetVMs.
            IQueryable<BookingPetVM> bookingPetVMs = from p in _db.Pets
                                                     select new BookingPetVM
                                                     {
                                                         PetId = p.PetId,
                                                         Name = p.Name
                                                     };

            // Assign the appropriate BookingPetVM to each BookingVM, along with Sitter names.
            foreach (var booking in bookingsTomarrowList)
            {
                List<int> petIds = _db.BookingPets.Where(bp => bp.BookingId == booking.BookingId).Where(bp => bp.PetId.HasValue).Select(bp => bp.PetId.GetValueOrDefault()).ToList();

                List<BookingPetVM> pets = new List<BookingPetVM>();
                foreach (var petId in petIds)
                {
                    pets.Add(bookingPetVMs.Where(pv => pv.PetId == petId).FirstOrDefault());
                }

                booking.Pets = pets;

                // Add sitter name.
                Sitter sitter = _db.Sitters.Where(s => s.SitterId == booking.SitterId).FirstOrDefault();
                User user = _db.Users.Where(u => u.UserId == sitter.UserId).FirstOrDefault();
                booking.SitterName = user.FirstName;
            }

            return bookingsTomarrowList;
        }

        public List<BookingVM> GetPastBookingVMsByUserId(int userID)
        {
            List<BookingVM> myBookings = GetBookingVMsByUserId(userID);
            List<BookingVM> pastBookings = new List<BookingVM>();

            // Filter for past bookings only
            foreach (var booking in myBookings)
            {
                if (booking.EndDate < DateTime.Now)
                {
                    pastBookings.Add(booking);
                }
            }

            // Sort most recent to oldest
            pastBookings = pastBookings.OrderByDescending(b => b.StartDate).ToList();

            return pastBookings;
        }

        public BookingFormVM GetBookingFormVM(int bookingID)
        {
            BookingVM booking = GetAllBookingVMs().Where(b => b.BookingId == bookingID).FirstOrDefault();
            BookingFormVM bookingForm = new BookingFormVM();
            bookingForm.BookingId = bookingID;
            bookingForm.SitterId = booking.SitterId;
            bookingForm.SitterName = booking.SitterName;
            bookingForm.Pets = booking.Pets;
            bookingForm.StartDate = booking.StartDate;
            bookingForm.EndDate = booking.EndDate;
            bookingForm.SpecialRequests = booking.SpecialRequests;

            return bookingForm;
        }

        public List<BookingPetVM> GetBookingPetVMsByUserId(int userId)
        {
            IQueryable<BookingPetVM> bookingPetVMs = from p in _db.Pets
                                                     where p.UserId == userId
                                                     select new BookingPetVM
                                                     {
                                                         PetId = p.PetId,
                                                         Name = p.Name
                                                     };

            List<BookingPetVM> result = bookingPetVMs.ToList();

            return result;
        }


        public List<Booking> GetBookingsBySitter(int sitterID)
        {
            var bookings = _db.Bookings.Where(b => b.SitterId == sitterID && b.PaymentId != null).ToList();
            return bookings;
        }

        public List<DateTime> GetBookedDates(List<Booking> bookings)
        {
            var bookedDates = new List<DateTime>();
            foreach (var booking in bookings)
            {
                for (DateTime date = (DateTime)booking.StartDate; date <= (DateTime)booking.EndDate; date = date.AddDays(1))
                {
                    bookedDates.Add(date);
                }
            }
            return bookedDates;
        }

        public bool CheckSitterAvailability(BookingFormVM booking)
        {
            // Get dates that sitter has open for new bookings.
            AvailabilityRepo availabilityRepo = new AvailabilityRepo(_db);
            var availabilities = availabilityRepo.GetAvailabilities(booking.SitterId);
            var bookings = GetBookingsBySitter(booking.SitterId);
            var bookedDates = GetBookedDates(bookings);
            var availableDates = availabilityRepo.GetAvailableDates(availabilities);
            var openDates = availableDates.Except(bookedDates);

            // Get all the dates of the booking.
            var bookingDates = new List<DateTime>();
            for (DateTime date = booking.StartDate; date <= booking.EndDate; date = date.AddDays(1))
            {
                bookingDates.Add(date);
            }

            // Check if any booking dates are dates that the sitter does not have open.
            var unfulfilledDates = bookingDates.Except(openDates).ToList();

            // If all booking dates are open, return true.
            if (unfulfilledDates.Count == 0)
            {
                return true;
            }
            else // If not all booking dates are open, return false.
            {
                return false;
            }
        }


        // SECTION: Create and Update Methods

        public int Create(BookingFormVM booking, int userId)
        {
            // Create a new Booking object.
            Booking newBooking = new Booking(booking.StartDate, booking.EndDate, booking.SpecialRequests, (int)booking.SitterId, userId);

            // List pets in booking.
            List<BookingPetVM> pets = new List<BookingPetVM>();
            foreach (var pet in booking.Pets)
            {
                if (pet.IsChecked)
                {
                    pets.Add(pet);
                }
            }

            // Add price to booking.
            newBooking = AddPriceToBooking(newBooking, pets.Count);

            // Save to database.
            _db.Add(newBooking);
            _db.SaveChanges();

            // Create BookingPet objects and add to database.
            foreach (var pet in pets)
            {
                BookingPet bookingPet = new BookingPet(newBooking.BookingId, pet.PetId);
                _db.Add(bookingPet);
                _db.SaveChanges();
            }

            return newBooking.BookingId;
        }

        public int CreateBookByUser(BookingFormVM booking, int userId)
        {
            // Create a new Booking object.
            Booking Booking = new Booking(booking.StartDate, booking.EndDate, booking.SpecialRequests, (int)booking.SitterId, userId);
            Booking.SitterId = 1;
            // List pets in booking.
            List<BookingPetVM> pets = new List<BookingPetVM>();
            foreach (var pet in booking.Pets)
            {
                if (pet.IsChecked)
                {
                    pets.Add(pet);
                }
            }

            // Add price to booking.
            Booking = AddPriceToBooking_(Booking, pets.Count);


            // Save to database.
            _db.Add(Booking);
            _db.SaveChanges();

            // Create BookingPet objects and add to database.
            foreach (var pet in pets)
            {
                BookingPet bookingPet = new BookingPet(Booking.BookingId, pet.PetId);
                _db.Add(bookingPet);
                _db.SaveChanges();
            }

            return Booking.BookingId;
        }

        public int CreateBookBySitter(BookingFormVM booking, int sitterId, int userId)
        {
            // Create a new Booking object.
            Booking Booking = new Booking(booking.StartDate, booking.EndDate, booking.SpecialRequests, sitterId, userId);

            // List pets in booking.

            // Add price to booking.
            // Add price to booking.
            int petsCount = 1;
            Booking = AddPriceToBooking_(Booking, petsCount);


            // Save to database.
            _db.Add(Booking);
            _db.SaveChanges();

            // Create BookingPet objects and add to database.
          

            return Booking.BookingId;
        }

        public int Update(BookingFormVM bookingForm)
        {
            // Get the booking to be updated.
            Booking booking = GetBooking(bookingForm.BookingId);

            // Update the properties.
            booking.StartDate = bookingForm.StartDate;
            booking.EndDate = bookingForm.EndDate;
            booking.SpecialRequests = bookingForm.SpecialRequests;

            // List pets in booking.
            List<BookingPetVM> pets = new List<BookingPetVM>();
            foreach (var pet in bookingForm.Pets)
            {
                if (pet.IsChecked)
                {
                    pets.Add(pet);
                }
            }

            // Recalculate price and update on the booking.
            booking = AddPriceToBooking(booking, pets.Count);

            // Save changes to the database.
            _db.Update(booking);
            _db.SaveChanges();

            // Delete all previous BookingPet records.
            foreach (var pet in bookingForm.Pets)
            {
                // Get the BookingPet record.
                BookingPet bookingPet = _db.BookingPets.Where(b => b.BookingId == bookingForm.BookingId && b.PetId == pet.PetId).FirstOrDefault();

                // Delete from the database.
                _db.Remove(bookingPet);
                _db.SaveChanges();
            }

            // Add BookingPet records.
            foreach (var pet in pets)
            {
                BookingPet bookingPet = new BookingPet(booking.BookingId, pet.PetId);
                _db.Add(bookingPet);
                _db.SaveChanges();
            }

            return booking.BookingId;
        }

        public int CreateBookingAnnouncement(int BookingId)
        {
            // Get the booking to be updated.
            Booking booking = GetBooking(BookingId);

            // Update the properties.
            booking.SitterId = 47;
            
            // Save changes to the database.
            _db.Update(booking);
            _db.SaveChanges();


            return booking.BookingId;
        }

        public bool CheckPetSelection(BookingFormVM bookingForm)
        {
            int selectedPets = 0;
            foreach (var pet in bookingForm.Pets)
            {
                if (pet.IsChecked)
                {
                    selectedPets++;
                }
            }

            if (selectedPets > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Booking AddPriceToBooking_(Booking booking, int petsCount)
        {
            // Convert nullable data types.
            DateTime startDate = (DateTime)booking.StartDate;
            DateTime endDate = (DateTime)booking.EndDate;

            // Calculate number of days in booking.
            int days = endDate.Subtract(startDate).Days + 1;

            // Get sitter.
           
            decimal price = 0 * days * petsCount;

            booking.Price = price;

            return booking;
        } 

        public Booking AddPriceToBooking(Booking booking, int petsCount)
        {
            // Convert nullable data types.
            DateTime startDate = (DateTime)booking.StartDate;
            DateTime endDate = (DateTime)booking.EndDate;

            // Calculate number of days in booking.
            int days = endDate.Subtract(startDate).Days + 1;

            // Get sitter.
            CsFacingSitterRepo sitterRepo = new CsFacingSitterRepo(_db);
            SitterVM sitter = sitterRepo.GetSitterVM((int)booking.SitterId);

            decimal price = sitter.Rate * days * petsCount;

            booking.Price = price;

            return booking;
        }

    
        public IPN AddTransaction(IPN ipn, string email)
        {
            // Add IPN record.
            _db.IPNs.Add(ipn);
            _db.SaveChanges();

            // Update Booking record with payment ID.
            Booking booking = _db.Bookings.Where(b => b.BookingId.ToString() == ipn.custom).FirstOrDefault();
            booking.PaymentId = ipn.paymentID;
            _db.Bookings.Update(booking);
            _db.SaveChanges();

            // Send confirmation email to the customer.
            SendConfirmationEmail(email, booking);

            return ipn;
        }

        public async Task SendConfirmationEmail(string email, Booking booking)
        {
            // Получить BookingVM
            BookingVM bookingVM = GetBookingVM(booking.BookingId);

            // Создать строку для животных
            string pets = "<ul>";
            foreach (var pet in bookingVM.Pets)
            {
                pets += $"<li>{pet.Name}</li>";
            }

            // Конвертировать Nullable значения
            DateTime startDate = (DateTime)booking.StartDate;
            DateTime endDate = (DateTime)booking.EndDate;
            decimal price = (decimal)bookingVM.Price;

            // Отправка электронного письма
            //var isEmailSent = await _emailService.SendSingleEmail(new ComposeEmailModel
            //{
            //    FirstName = "TeamGreen",
            //    LastName = "SSD",
            //    Subject = "Booking Confirmed!",
            //    Email = email,
            //    Body = $"<h1 style=\"font-weight:normal;\">Thank you for booking with Sitter Care!</h1>" +
            //           $"<h2>Booking Details</h2>" +
            //           $"<ul><li>Sitter: <span style=\"font-weight:normal;\">{bookingVM.SitterName}<span></li>" +
            //           $"<li>Pets: <span style=\"font-weight:normal;\">{pets}<span></li>" +
            //           $"<li>Start Date: <span style=\"font-weight:normal;\">{startDate.ToString("MMMM dd, yyyy")}</span></li>" +
            //           $"<li>End Date: <span style=\"font-weight:normal;\">{endDate.ToString("MMMM dd, yyyy")}</span></li>" +
            //           $"<li>Special Requests: <span style=\"font-weight:normal;\">{booking.SpecialRequests}</span></li>" +
            //           $"<li>Price: <span style=\"font-weight:normal;\">{price.ToString("C")}</span></li>" +
            //           $"<li>Payment Confirmation: <span style=\"font-weight:normal;\">{booking.PaymentId}</span></li></ul>"
            //});

            //// Можно обработать результат отправки письма
            //if (isEmailSent)
            //{
            //    // Логика в случае успешной отправки
            //}
            //else
            //{
            //    // Логика в случае неудачной отправки
            //}
        }


        public bool CheckDate(DateTime startDate)
        {
            DateTime today = DateTime.Today;
            
            if (startDate > today)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
