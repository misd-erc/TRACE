using System.Security.Claims;

namespace TRACE.Helpers
{
    public class CurrentUserHelper
    {
        private readonly ClaimsPrincipal _user;

        public CurrentUserHelper(ClaimsPrincipal user)
        {
            _user = user;
        }

        public string Name => _user.FindFirst("name")?.Value ?? "Unknown";
        public string Email => _user.FindFirst("preferred_username")?.Value ?? "Unknown";
    }
}
