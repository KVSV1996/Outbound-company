using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;
using Outbound_company.Services;

namespace Outbound_company.Controllers
{
    public class CompaniesController : Controller
    {

        private ICompaniesService _companiesService;
        public CompaniesController(ICompaniesService companiesService)
        {
            _companiesService = companiesService ?? throw new ArgumentNullException(nameof(companiesService));
        }

        public async Task<IActionResult> Index()
        {
            return View(_companiesService.GetAllCompanies());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Channel,Extension,Context,CallerId")] OutboundCompany outboundCompany)
        {
            if (ModelState.IsValid)
            {
                _companiesService.InsertCompany(outboundCompany);
                return RedirectToAction(nameof(Index));
            }
            return View(outboundCompany);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var outboundCompany = _companiesService.GetCompanyById(id);
            if (outboundCompany == null)
            {
                return NotFound();
            }
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
                    _companiesService.UpdateCompany(outboundCompany);
                }
                catch (DbUpdateConcurrencyException)
                {
                     return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(outboundCompany);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var outboundCompany = _companiesService.GetCompanyById(id);
            if (outboundCompany == null)
            {
                return NotFound();
            }

            return View(outboundCompany);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _companiesService.DeleteCompany(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
