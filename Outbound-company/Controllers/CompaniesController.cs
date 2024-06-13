using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using Serilog;

namespace Outbound_company.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {

        private ICompaniesService _companiesService;
        private INumberService _numberService;
        public CompaniesController(ICompaniesService companiesService, INumberService numberService)
        {
            _companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
            _numberService = numberService ?? throw new ArgumentNullException( nameof(numberService));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _companiesService.GetAllCompaniesAsync());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.NumberPools = new SelectList(await _numberService.GetAllNumberPoolsAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Channel,Extension,Context,CallerId,NumberPoolId,TrunkType")] OutboundCompany outboundCompany)
        {
            if (ModelState.IsValid)
            {
                await _companiesService.InsertCompanyAsync(outboundCompany);
                Log.Information($"Company {outboundCompany.Name} created successfully.");
                return RedirectToAction(nameof(Index));
            }
            Log.Warning("Failed to create company due to invalid model state.");
            return View(outboundCompany);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var outboundCompany = await _companiesService.GetCompanyByIdAsync(id);
            if (outboundCompany == null)
            {
                Log.Warning($"Company with ID {id} not found.");
                return NotFound();
            }
            ViewBag.NumberPools = new SelectList(await _numberService.GetAllNumberPoolsAsync(), "Id", "Name");
            return View(outboundCompany);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Channel,Extension,Context,CallerId")] OutboundCompany outboundCompany)
        {
            outboundCompany.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    await _companiesService.UpdateCompanyAsync(outboundCompany);
                    Log.Information($"Company {outboundCompany.Name} updated successfully.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Error(ex, $"Concurrency error updating company with ID {id}.");
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            Log.Warning($"Failed to update company {outboundCompany.Name} due to invalid model state.");
            return View(outboundCompany);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var outboundCompany = await _companiesService.GetCompanyByIdAsync(id);
            if (outboundCompany == null)
            {
                Log.Warning($"Company with ID {id} not found.");
                return NotFound();
            }

            return View(outboundCompany);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _companiesService.DeleteCompanyAsync(id);
            Log.Information($"Company with ID {id} deleted successfully.");
            return RedirectToAction(nameof(Index));
        }

    }
}
