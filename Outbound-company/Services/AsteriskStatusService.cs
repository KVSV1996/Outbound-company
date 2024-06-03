using Microsoft.Extensions.Options;
using Outbound_company.Models.Enums;
using Outbound_company.Models;
using System.Net.Http.Headers;
using System.Text;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Services
{
    public class AsteriskStatusService : IAsteriskStatusService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly AsteriskSettings _asteriskSettings;
        private readonly AsteriskStatusModel _statusModel = new AsteriskStatusModel();

        public AsteriskStatusService( IOptions<AsteriskSettings> asteriskSettings)
        {

            _asteriskSettings = asteriskSettings.Value;
        }

        public async Task UpdateStatusAsync()
        {
            try
            {
                var requestUri = $"http://{_asteriskSettings.Url}/ari/asterisk/ping";
                var username = _asteriskSettings.Username;
                var password = _asteriskSettings.Password;
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                if (content.Contains("\"ping\": \"pong\""))
                {
                    _statusModel.Status = AsteriskStatus.Online;
                }
                else
                {
                    _statusModel.Status = AsteriskStatus.Offline;
                }
            }
            catch
            {
                _statusModel.Status = AsteriskStatus.Offline;
            }
        }

        public AsteriskStatusModel GetStatus()
        {
            return _statusModel;
        }
    }
}
