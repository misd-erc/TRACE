using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using TRACE.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace TRACE.Helpers
{
    public class CurrentUserHelper
    {
        private readonly ClaimsPrincipal _user;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;
        private readonly ErcdbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserHelper(ClaimsPrincipal user, ITokenAcquisition tokenAcquisition, IConfiguration configuration, ErcdbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _user = user;
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public string Name => _user.FindFirst("name")?.Value ?? "Unknown";
        public string Email => _user.FindFirst("preferred_username")?.Value ?? "Unknown";
        public string Role => _user.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";

        private string _department = "Unknown";

        public string GetCurrentUsername()
        {
            var email = _user.FindFirst("preferred_username")?.Value ?? "Unknown";
            return email.Split('@')[0];
        }

        public async Task<string> GetDepartmentAsync()
        {
            if (_department != "Unknown") return _department;

            try
            {
                string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "User.Read" });

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me?$select=department");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        using JsonDocument doc = JsonDocument.Parse(content);
                        _department = doc.RootElement.GetProperty("department").GetString() ?? "Unknown";
                    }
                }
            }
            catch
            {
                _department = "Unknown";
            }

            return _department;
        }

        public async Task<string> GetUserRoleAsync()
        {
            if (!string.IsNullOrEmpty(Role) && Role != "Unknown") return Role;

            if (string.IsNullOrEmpty(Email)) return "Unknown";

            try
            {
                var user = await _context.Users
                    .Where(u => u.Email == Email)
                    .Select(u => u.UserCategory)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(user))
                {
                    var claimsIdentity = _user.Identity as ClaimsIdentity;
                    if (claimsIdentity != null)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user));

                        var authenticationManager = _httpContextAccessor.HttpContext;
                        if (authenticationManager != null)
                        {
                            await authenticationManager.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        }
                    }
                }

                return user ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
