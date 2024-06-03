using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Outbound_company.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Outbound_company.Services
{
    public class AsteriskCountOfCallsService : IAsteriskCountOfCallsService
    {
        private readonly AsteriskSettings _asteriskSettings;

        public AsteriskCountOfCallsService(IOptions<AsteriskSettings> asteriskSettings)
        {
            _asteriskSettings = asteriskSettings.Value ?? throw new ArgumentNullException(nameof(asteriskSettings.Value));
        }
        public async Task<int> GetActiveCallsAsync()
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"http://{_asteriskSettings.Url}/ari/channels";
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_asteriskSettings.Username}:{_asteriskSettings.Password}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
                HttpResponseMessage response = await client.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var channels = JsonConvert.DeserializeObject<List<dynamic>>(content);
                    return channels.Count;
                }
                else
                {
                    throw new Exception($"Failed to fetch channels: {response.StatusCode}");
                }
            }
        }
    }
}
