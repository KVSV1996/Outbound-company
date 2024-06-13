using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Outbound_company.Models;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace Outbound_company.Controllers
{
    [Authorize]
    public class CallController : Controller
    {
        private readonly AsteriskSettings _asteriskSettings;
        private static readonly HttpClient client = new HttpClient();
        public CallController(IOptions<AsteriskSettings> asteriskSettings)
        {
            _asteriskSettings = asteriskSettings.Value;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MakeCall(string typeOfTrunk, string phoneNumber, string channel, string extension, string context, string callerId)
        {
            try
            {
                var requestUri = $"http://{_asteriskSettings.Url}/ari/channels";
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_asteriskSettings.Username}:{_asteriskSettings.Password}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var jsonContent = new
                {
                    endpoint = $"{typeOfTrunk}/{phoneNumber}@{channel}",
                    extension = extension,
                    context = context,
                    callerId = callerId
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Call initiated successfully!";
                    Log.Information($"Test call. Call initiated successfully to {phoneNumber} using trunk {typeOfTrunk}/{channel}.");

                }
                else
                {
                    ViewBag.Message = $"Failed to initiate call. Status code: {response.StatusCode}";
                    Log.Warning($"Failed to initiate call to {phoneNumber}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                Log.Error(ex, $"Error initiating call to {phoneNumber} using trunk {typeOfTrunk}/{channel}.");
            }

            return View("Index");
        }
    }
}
