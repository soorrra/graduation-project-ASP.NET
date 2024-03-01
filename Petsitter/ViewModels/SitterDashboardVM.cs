using GoogleMaps.LocationServices;
using Petsitter.Models;
using System.ComponentModel;

namespace Petsitter.ViewModels
{
    public class SitterDashboardVM
    {
        public int bookingId { get; set; }

        public string? sitter { get; set; }
        [DisplayName("Pet Parent")]

        public string? petParent { get; set; }
        public MapPoint? PointCustomer { get; set; }
        public MapPoint? PointSitter { get; set; }
        [DisplayName("Pet Type")]

        public string? petType { get; set; }
        [DisplayName("Start Date")]

        public string? startDate { get; set; }
        [DisplayName("End Date")]

        public string? endDate { get; set; }
        [DisplayName("Status")]

        public string? status { get; set; }
        [DisplayName("Review")]

        public string? review { get; set; }
       public User user { get; set; }

        public int completeNbr { get; set; }
        public int  upComingNbr { get; set; }

        public int reviewsNbr { get; set; }



    }
}
