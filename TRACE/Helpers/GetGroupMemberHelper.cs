using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TRACE.Helpers
{
    public class GetGroupMemberHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _httpClient;

        public GetGroupMemberHelper(IConfiguration configuration, ITokenAcquisition tokenAcquisition, HttpClient httpClient)
        {
            _configuration = configuration;
            _tokenAcquisition = tokenAcquisition;
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetGroupMemberEmailsAsync()
        {
            var groupId = _configuration["AzureAd:GroupId"];
            var emails = new List<string>();

            try
            {
                string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "https://graph.microsoft.com/.default" });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync($"https://graph.microsoft.com/v1.0/groups/{groupId}/members?$select=mail");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(content);

                    foreach (var element in doc.RootElement.GetProperty("value").EnumerateArray())
                    {
                        if (element.TryGetProperty("@odata.type", out var type) && type.GetString() == "#microsoft.graph.user")
                        {
                            var email = element.GetProperty("mail").GetString();
                            if (!string.IsNullOrEmpty(email))
                            {
                                emails.Add(email);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching group members: {ex.Message}");
            }

            return emails;
        }
    }
}
