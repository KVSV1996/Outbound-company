using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Outbound_company.Controllers
{
    public class CompanyManagementController : Controller
    {
        private readonly ICallsManagementService _callsManagementService;
        private readonly IAsteriskCountOfCallsService _countOfCallsService;
        private readonly ICompaniesService _companiesService;
        private readonly AsteriskSettings _asteriskSettings;
        private readonly INumberService _numberService;
        private static readonly HttpClient client = new HttpClient();

        public CompanyManagementController(ICallsManagementService callsManagementService, ICompaniesService companiesService, INumberService numberService, IOptions<AsteriskSettings> asteriskSettings, IAsteriskCountOfCallsService countOfCallsService)
        {
            _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
            _companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
            _asteriskSettings = asteriskSettings.Value ?? throw new ArgumentNullException(nameof(asteriskSettings.Value));
            _countOfCallsService = countOfCallsService ?? throw new ArgumentNullException(nameof(countOfCallsService));
            _callsManagementService = callsManagementService ?? throw new ArgumentNullException(nameof(callsManagementService));
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

        public async Task<IActionResult> Start(int id)
        {
            var company = _companiesService.GetCompanyById(id);
            var numberPool = _numberService.GetById(company.NumberPoolId);
            _callsManagementService.Start(company, numberPool);
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Stop(int id)
        {
            _callsManagementService.Stop();
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
