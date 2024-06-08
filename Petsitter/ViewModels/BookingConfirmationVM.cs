using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petsitter.ViewModels
{
    public class BookingConfirmationVM
    {

            public int BookingID { get; set; }
            public bool Confirmed { get; set; }
        

    }
}
