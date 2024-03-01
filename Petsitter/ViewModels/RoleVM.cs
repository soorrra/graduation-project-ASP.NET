using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Petsitter.ViewModels
{
    public class RoleVM
    {
        // aspnetroles
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

    }
}