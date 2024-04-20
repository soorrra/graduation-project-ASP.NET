using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public partial class ServiceType
    {
        public ServiceType()
        {
             Sitters = new HashSet<Sitter>();
        }

        public string ServiceType1 { get; set; } = null!;

        public virtual ICollection<Sitter> Sitters { get; set; }
    }
}
