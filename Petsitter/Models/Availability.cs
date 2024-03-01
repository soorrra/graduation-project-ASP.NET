using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public partial class Availability
    {
        public Availability()
        {
            Sitters = new HashSet<Sitter>();
        }

        public int AvailabilityId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Sitter> Sitters { get; set; }
    }
}
