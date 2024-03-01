using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Petsitter.Models
{
    public class IPN
    {
        // This lets you link the request to paypal with the response.
        public string custom { get; set; }

        [Display(Name = "ID")]
        [Key] // Define primary key.
        public string paymentID { get; set; }
        public string cart { get; set; }
        public string create_time { get; set; }

        // Payer data.
        public string payerID { get; set; }
        public string payerFirstName { get; set; }
        public string payerLastName { get; set; }
        public string payerMiddleName { get; set; }
        public string payerEmail { get; set; }
        public string payerCountryCode { get; set; }
        public string payerStatus { get; set; }

        // Payment data.
        public string amount { get; set; }
        public string currency { get; set; }
        public string intent { get; set; }
        public string paymentMethod { get; set; }
        public string paymentState { get; set; }
    }
}
