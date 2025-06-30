using System.ComponentModel.DataAnnotations;

namespace TRACE.Models
{
    public class UserRolesPerModule
    {
        public int RoleId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = null!;

        [Display(Name = "Module Name")]
        public string ModuleName { get; set; } = null!;

        [Display(Name = "Can View")]
        public bool? CanView { get; set; }

        [Display(Name = "Can Edit")]
        public bool? CanEdit { get; set; }

        [Display(Name = "Can Add/Create")]
        public bool? CanCreate { get; set; }

        [Display(Name = "Date")]
        public DateTime? AssignedAt { get; set; }
    }
}
