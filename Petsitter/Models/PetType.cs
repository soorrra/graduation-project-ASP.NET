using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public partial class PetType
    {
        public PetType()
        {
            Pets = new HashSet<Pet>();
            Sitters = new HashSet<Sitter>();
        }

        public string PetType1 { get; set; } = null!;

        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<Sitter> Sitters { get; set; }
    }
}
