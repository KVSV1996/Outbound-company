using Microsoft.AspNetCore.Mvc;
using Outbound_company.Models;
using OfficeOpenXml;
using Outbound_company.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Serilog;


namespace Outbound_company.Controllers
{
    [Authorize]
    public class NumberPoolsController : Controller
    {
        private readonly INumberService _numberService;

        public NumberPoolsController(INumberService numberService)
        {
            _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
        }

        public async Task<IActionResult> Index()
        {
            var pools = await _numberService.GetAllNumberPoolsAsync();
            return View(pools);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NumberPool numberPool, IFormFile excelFile)
        {
            var phoneNumbers = new List<PhoneNumber>();

            if (excelFile != null && excelFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 1; row <= rowCount; row++)
                        {
                            var phoneNumber = worksheet.Cells[row, 1].Text;
                            phoneNumbers.Add(new PhoneNumber { Number = phoneNumber });
                        }
                    }
                }

                numberPool.PhoneNumbers = phoneNumbers;
                await _numberService.InsertNumberPoolsAsync(numberPool);
                Log.Information("The number pool has been successfully added");

                return RedirectToAction(nameof(Index));
            }

            return View(numberPool);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var numberPool = await _numberService.GetByIdAsync(id);
            if (numberPool == null)
            {
                return NotFound();
            }

            return View(numberPool);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _numberService.DeleteNumberPoolsAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
