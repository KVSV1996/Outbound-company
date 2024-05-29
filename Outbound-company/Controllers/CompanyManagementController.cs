using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Outbound_company.Models;
using Outbound_company.Services;
using System.Net.Http.Headers;
using System.Text;

namespace Outbound_company.Controllers
{
    public class CompanyManagementController : Controller
    {
        private readonly ICompaniesService _companiesService;
        private readonly AsteriskSettings _asteriskSettings;
        private readonly INumberService _numberService;
        private static readonly HttpClient client = new HttpClient();

        public CompanyManagementController(ICompaniesService companiesService, INumberService numberService, IOptions<AsteriskSettings> asteriskSettings)
        {
            _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
            _companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
            _asteriskSettings = asteriskSettings.Value ?? throw new ArgumentNullException(nameof(asteriskSettings.Value));
        }

        public async Task<IActionResult> Details(int id)
        {
            var company = _companiesService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> MakeCall(int id)
        {
            try
            {
                var company =  _companiesService.GetCompanyById(id);
                if (company == null)
                {
                    ViewBag.Message = "Company not found.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var numberPool = _numberService.GetById(company.NumberPoolId);
                if (numberPool == null)
                {
                    ViewBag.Message = "Number pool not found.";
                    return RedirectToAction(nameof(Details), new { id });
                }

                var requestUri = _asteriskSettings.Url + "/ari/channels";
                var username = _asteriskSettings.Username;
                var password = _asteriskSettings.Password;
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                foreach (var phoneNumber in numberPool.PhoneNumbers)
                {
                    var endpoint = $"{company.TrunkType}/{phoneNumber.Number}@{company.Channel}";

                    var jsonContent = new
                    {
                        endpoint = endpoint,
                        extension = company.Extension,
                        context = company.Context,
                        callerId = company.CallerId
                    };

                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(requestUri, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = $"Failed to initiate call to {phoneNumber.Number}. Status code: {response.StatusCode}";
                        break; // Прервать цикл в случае ошибки
                    }
                }

                ViewBag.Message = "Call initiated successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Start(int id)
        {
            // Логика для запуска компании
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Stop(int id)
        {
            // Логика для остановки компании
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
