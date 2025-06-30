namespace TRACE.Models
{
    public class UserManagementViewModel
    {
        public IEnumerable<TRACE.Models.User> Users { get; set; }
        public IEnumerable<TRACE.Models.UserRolesPerModule> UserRolesPerModule { get; set; }
    }
}
