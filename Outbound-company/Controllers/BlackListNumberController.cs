using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using Serilog;

namespace Outbound_company.Controllers
{
    [Authorize]
    public class BlackListNumberController : Controller
    {

        private readonly IBlackListNumberService _service;

        public BlackListNumberController(IBlackListNumberService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var totalItems = await _service.GetCountAsync();
            var blackListNumbers = await _service.GetAllAsync();

            var viewModel = new BlackListNumberViewModel
            {
                BlackListNumbers = blackListNumbers,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlackListNumber blackListNumber)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(blackListNumber);
                Log.Information($"BlackListNumber with number {blackListNumber.Number} created successfully.");
                return RedirectToAction(nameof(Index));
            }
            Log.Warning("Failed to create BlackListNumber due to invalid model state.");
            return View(blackListNumber);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blackListNumber = await _service.GetByIdAsync(id);
            if (blackListNumber == null)
            {
                Log.Information($"BlackListNumber with id {id} not found for editing.");
                return NotFound();
            }
            return View(blackListNumber);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BlackListNumber blackListNumber)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(blackListNumber);
                Log.Information($"BlackListNumber with id {blackListNumber.Id} updated successfully.");
                return RedirectToAction(nameof(Index));
            }
            Log.Warning($"Failed to update BlackListNumber with id {blackListNumber.Id} due to invalid model state.");
            return View(blackListNumber);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blackListNumber = await _service.GetByIdAsync(id);
            if (blackListNumber == null)
            {
                Log.Warning($"BlackListNumber with id {id} not found for deletion.");
                return NotFound();
            }
            return View(blackListNumber);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            Log.Information($"BlackListNumber with id {id} deleted successfully.");
            return RedirectToAction(nameof(Index));
        }
    }
}
