namespace TRACE.Models
{
    public class UserRolesPerModule
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public string ModuleName { get; set; } = null!;

        public bool? CanView { get; set; }

        public bool? CanEdit { get; set; }

        public bool? CanCreate { get; set; }

        public DateTime? AssignedAt { get; set; }
    }
}
