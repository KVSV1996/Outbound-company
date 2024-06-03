using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Controllers
{
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
            return View(_companiesService.GetAllCompanies());
        }
        public IActionResult Create()
        {
            ViewBag.NumberPools = new SelectList(_numberService.GetAllNumberPools(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Channel,Extension,Context,CallerId,NumberPoolId,TrunkType")] OutboundCompany outboundCompany)
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
            ViewBag.NumberPools = new SelectList(_numberService.GetAllNumberPools(), "Id", "Name");
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
        public async Task<IActionResult> Delete(int id)
        {
            var outboundCompany = _companiesService.GetCompanyById(id);
            if (outboundCompany == null)
            {
                return NotFound();
            }

            return View(outboundCompany);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _companiesService.DeleteCompany(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
