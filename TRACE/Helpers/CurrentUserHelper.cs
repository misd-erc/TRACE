using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;

namespace TRACE.Helpers
{
    public class CurrentUserHelper
    {
        private readonly ClaimsPrincipal _user;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;

        public CurrentUserHelper(ClaimsPrincipal user, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _user = user;
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        public string Name => _user.FindFirst("name")?.Value ?? "Unknown";
        public string Email => _user.FindFirst("preferred_username")?.Value ?? "Unknown";

        private string _department = "Unknown";

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
    }
}
