
namespace Petsitter.Models
{
    public class EmailData
    {
        public string From { get; set; }
       
        public string? To { get; set; }
       
        public string? Subject { get; set; }
        
        public string? Body { get; set; }

        public string Password { get; set; }

        public EmailData()
        {           
            From = "petsitterdiplom@gmail.com";
            Password = "slkd vppk khqq mmoe";                                  
        }
    }
}
