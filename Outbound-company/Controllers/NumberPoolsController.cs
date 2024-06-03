using Microsoft.AspNetCore.Mvc;
using Outbound_company.Models;
using OfficeOpenXml;
using Outbound_company.Services.Interfaces;


namespace Outbound_company.Controllers
{
    public class NumberPoolsController : Controller
    {
        private readonly INumberService _numberService;

        public NumberPoolsController(INumberService numberService)
        {
            _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
        }

        public IActionResult Index()
        {
            var pools = _numberService.GetAllNumberPools();
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
                _numberService.InsertNumberPools(numberPool);
                return RedirectToAction(nameof(Index));
            }

            return View(numberPool);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var numberPool = _numberService.GetById(id);
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
            _numberService.DeleteNumberPools(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
