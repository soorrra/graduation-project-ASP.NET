using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Petsitter.Data.Services;
using Petsitter.Repositories;
using MailKit.Net.Smtp;
using Petsitter.Services;
using SendGrid.Helpers.Mail;
using Petsitter.ViewModels;

namespace Petsitter.Services
{
    public class BookingReminderService : BackgroundService
    {
        private readonly EmailSettings _emailSettings;
        private readonly BookingRepo _bookingRepo;
        private readonly CustomerRepo _customerRepo;
        private BookingRepo bookingRepo;

        public BookingReminderService(BookingRepo bookingRepo)
        {
            this.bookingRepo = bookingRepo;
        }

        public BookingReminderService(IOptions<EmailSettings> emailSettings, BookingRepo bookingRepo, CustomerRepo customerRepo)
        {
            _emailSettings = emailSettings.Value;
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<BookingVM> bookingsTomorrow = _bookingRepo.GetBookingsStartingTomorrow();

                foreach (var booking in bookingsTomorrow)
                {
                    var user = _customerRepo.GetUserData(booking.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        var mailRequest = new Mailrequest

                        {
                            ToEmail = user.Email,
                            Subject = "Напоминание о бронировании",
                            Body = $"Уважаемый {user.FirstName}, напоминаем Вам о бронировании с {booking.StartDate} по {booking.EndDate}."
                        };

                        var email = new MimeMessage();
                        email.Sender = MailboxAddress.Parse(_emailSettings.Email);
                        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                        email.Subject = mailRequest.Subject;
                        var builder = new BodyBuilder();


                        builder.HtmlBody = mailRequest.Body;
                        email.Body = builder.ToMessageBody();

                        using var smtp = new SmtpClient();
                        smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                        smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
                        try
                        {
                            await smtp.SendAsync(email);
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            Console.WriteLine("них не работает");
                        }
                        smtp.Disconnect(true);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }

}

