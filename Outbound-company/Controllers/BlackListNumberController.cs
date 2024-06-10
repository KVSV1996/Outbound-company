using Microsoft.AspNetCore.Mvc;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;
using System.Drawing.Printing;

namespace Outbound_company.Controllers
{
    public class BlackListNumberController : Controller
    {

        private readonly IBlackListNumberService _service;

        public BlackListNumberController(IBlackListNumberService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await _service.GetCountAsync();
            var blackListNumbers = await _service.GetAllByPagAsync(pageNumber, pageSize);

            var viewModel = new BlackListNumberViewModel
            {
                BlackListNumbers = blackListNumbers,
                PageNumber = pageNumber,
                PageSize = pageSize,
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
                return RedirectToAction(nameof(Index));
            }
            return View(blackListNumber);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blackListNumber = await _service.GetByIdAsync(id);
            if (blackListNumber == null)
            {
                return NotFound();
            }
            return View(blackListNumber);
        }


        public async Task<IActionResult> Edit(BlackListNumber blackListNumber)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(blackListNumber);
                return RedirectToAction(nameof(Index));
            }
            return View(blackListNumber);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blackListNumber = await _service.GetByIdAsync(id);
            if (blackListNumber == null)
            {
                return NotFound();
            }
            return View(blackListNumber);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
