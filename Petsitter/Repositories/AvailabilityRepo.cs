using Petsitter.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
namespace Petsitter.Repositories
{
    public class AvailabilityRepo
    {
        private readonly PetsitterContext _db;

        public AvailabilityRepo(PetsitterContext db)
        {
            _db = db;
        }

        public List<Availability> GetAvailabilities(int sitterID)
        {
            var availabilities = (from s in _db.Sitters
                                  from a in s.Availabilities
                                  where s.SitterId == sitterID
                                  select a).ToList();
            return availabilities;
        }
        public List<DateTime> GetAvailableDates(List<Availability> availabilities)
        {
            var availableDates = new List<DateTime>();
            foreach (var a in availabilities)
            {
                for (DateTime date = (DateTime)a.StartDate; date <= (DateTime)a.EndDate; date = date.AddDays(1))
                {
                    availableDates.Add(date);
                }
            }
            return availableDates;
        }

        public Tuple<int, string> AddAvailability(SitterAvailabilityVM availabilityVM)

        {
            var availabilities = GetAvailabilities(availabilityVM.SitterId);

            Availability availability = new Availability
            {
                StartDate = availabilityVM.StartDate,
                EndDate = availabilityVM.EndDate

            };
            var newAvailableDates = new List<DateTime>();
            //get all dates between the startDate and endDate of the new availability

            for (DateTime date = (DateTime)availability.StartDate; date <= (DateTime)availability.EndDate; date = date.AddDays(1))
            {
                newAvailableDates.Add(date);
            }

            var availableDates = new List<DateTime>();

            //get all dates between the startDate and endDate of all the availabalities of the sitter
            foreach (var a in availabilities)
            {
                for (DateTime date = (DateTime)a.StartDate; date <= (DateTime)a.EndDate; date = date.AddDays(1))
                {
                    availableDates.Add(date);
                }
            }
            //check if the existing availabilities contains any the new availability'date
            bool isAvailable = false;
            foreach (DateTime newDate in newAvailableDates)
            {
                if (availableDates.Contains(newDate))
                {
                    isAvailable = true; break;
                }
            }

            string message = String.Empty;

            try
            {
                //if not already exist then we add it 
                if (!isAvailable)
                {
                    // Add the availability to the database
                    _db.Availabilities.Add(availability);
                    _db.SaveChanges();

                    // Get the current sitter and add the availability to their list of availabilities
                    var currentSitter = _db.Sitters.Include(s => s.Availabilities).FirstOrDefault(s => s.SitterId == availabilityVM.SitterId);
                    currentSitter.Availabilities.Add(availability);
                    _db.SaveChanges();


                    message = $"Success adding new availability";
                }
                //if it already exist we display a message 
                else
                {
                    availability.AvailabilityId = 0;
                    message = $"Already Exist, Try to add new one";


                }

            }
            catch (Exception e)
            {
                availability.AvailabilityId = -1;

                message = e.Message + " "
    + "The sitter account may not exist or "
    + "there could be a foreign key restriction.";

            }

            return Tuple.Create(availability.AvailabilityId, message);
        }
    }
}
