using Petsitter.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class CustomerVM
    { 
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string UserType { get; set; }
        public List<BookingPet>? BookedPets { get; set; }
        public List<Booking>? Booking { get; set; }

        public IFormFile? ProfileImage { get; set; }


    }
}
