using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserManagementController(ITokenAcquisition tokenAcquisition, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        [Route("usermanagement")]
        public async Task<IActionResult> UserManagement()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");
            }

            var users = await GetUsersFromGroupAsync();
            return View(users);
        }

        private async Task<List<UserDto>> GetUsersFromGroupAsync()
        {
            string groupId = _configuration["AzureAd:GroupId"];
            var users = new List<UserDto>();

            try
            {
                string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "https://graph.microsoft.com/.default" });


                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"https://graph.microsoft.com/v1.0/groups/{groupId}/members?$select=id,displayName,mail,department");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(content);

                    foreach (var element in doc.RootElement.GetProperty("value").EnumerateArray())
                    {
                        if (element.TryGetProperty("@odata.type", out var type) && type.GetString() == "#microsoft.graph.user")
                        {
                            users.Add(new UserDto
                            {
                                DisplayName = element.GetProperty("displayName").GetString(),
                                Email = element.GetProperty("mail").GetString() ?? "N/A",
                                Department = element.GetProperty("department").GetString() ?? "N/A"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }

            return users;
        }
    }

    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
    }
}
