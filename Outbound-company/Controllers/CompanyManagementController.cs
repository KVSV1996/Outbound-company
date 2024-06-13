using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Outbound_company.Controllers
{
    [Authorize]
    public class CompanyManagementController : Controller
    {
        private readonly ICallsManagementService _callsManagementService;
        private readonly IAsteriskCountOfCallsService _countOfCallsService;
        private readonly ICompaniesService _companiesService;
        private readonly AsteriskSettings _asteriskSettings;
        private readonly INumberService _numberService;
        private readonly ICallStatisticsService _callStatisticsService;
        private readonly IBlackListNumberService _blackListNumberService;
        private static readonly HttpClient client = new HttpClient();


        public CompanyManagementController(ICallsManagementService callsManagementService, ICompaniesService companiesService, INumberService numberService, IOptions<AsteriskSettings> asteriskSettings, IAsteriskCountOfCallsService countOfCallsService, ICallStatisticsService callStatisticsService, IBlackListNumberService blackListNumberService)
        {
            _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
            _companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
            _asteriskSettings = asteriskSettings.Value ?? throw new ArgumentNullException(nameof(asteriskSettings.Value));
            _countOfCallsService = countOfCallsService ?? throw new ArgumentNullException(nameof(countOfCallsService));
            _callsManagementService = callsManagementService ?? throw new ArgumentNullException(nameof(callsManagementService));
            _callStatisticsService = callStatisticsService ?? throw new ArgumentNullException(nameof(callStatisticsService));
            _blackListNumberService = blackListNumberService ?? throw new ArgumentNullException(nameof(blackListNumberService));
        }

        public async Task<IActionResult> Details(int id)
        {
            var company = await _companiesService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var numberPool = await _numberService.GetByIdAsync(company.NumberPoolId);
            var totalNumbers = numberPool?.PhoneNumbers?.Count ?? 0;

            // Получаем количество вызовов для компании
            var callStatistics = await _callStatisticsService.GetStatysticByCompanyIdAsync(id);
            var completedCalls = callStatistics?.Count();

            // Вычисляем процент выполнения
            double completionPercentage = totalNumbers > 0 ? ((double)completedCalls / totalNumbers) * 100 : 0;

            // Передаем данные в ViewBag
            ViewBag.CompletionPercentage = completionPercentage;
            ViewBag.CompletedCalls = completedCalls;
            ViewBag.TotalNumbers = totalNumbers;

            return View(company);
        }


        public async Task<IActionResult> Start(int id)
        {
            NumberPool numberPool;
            var company = await _companiesService.GetCompanyByIdAsync(id);
            var latestStatisticByCompanyId = await _callStatisticsService.GetLatestByCompanyIdAsync(id);
            var blackListNumbers = await _blackListNumberService.GetAllAsync();


            if (latestStatisticByCompanyId!=null)
            {
                numberPool = new NumberPool
                {
                    Id = company.NumberPoolId,
                    PhoneNumbers = await _numberService.GetPhoneNumbersStartingFromAsync(latestStatisticByCompanyId.CompanyId, latestStatisticByCompanyId.PhoneNumberId+1)
                };
            }
            else
            {
                numberPool = await _numberService.GetByIdAsync(company.NumberPoolId); 
            }
            
            _callsManagementService.Start(company, numberPool, blackListNumbers, int.Parse(_asteriskSettings.MaximumCountOfCalls));
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Stop(int id)
        {
            _callsManagementService.Stop();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCompanyStatistics(int id)
        {
            await _callStatisticsService.DeleteStatysticByCompanyIdAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetUpdatedStatistics(int id)
        {
            var company = await _companiesService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var numberPool = await _numberService.GetByIdAsync(company.NumberPoolId);
            var totalNumbers = numberPool?.PhoneNumbers?.Count ?? 0;

            var callStatistics = await _callStatisticsService.GetStatysticByCompanyIdAsync(id);
            var completedCalls = callStatistics?.Count();

            double completionPercentage = totalNumbers > 0 ? ((double)completedCalls / totalNumbers) * 100 : 0;

            return Json(new
            {
                CompletionPercentage = completionPercentage.ToString("0.00"),
                CompletedCalls = completedCalls,
                TotalNumbers = totalNumbers
            });
        }
    }
}
