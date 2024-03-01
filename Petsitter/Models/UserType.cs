using System;
using System.Collections.Generic;

namespace Petsitter.Models
{
    public partial class UserType
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }

        public string UserType1 { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
