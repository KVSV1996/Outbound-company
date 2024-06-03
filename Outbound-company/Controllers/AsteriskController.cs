using Microsoft.AspNetCore.Mvc;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Controllers
{
    public class AsteriskController : Controller
    {
        private readonly IAsteriskStatusService _asteriskStatusService;

        public AsteriskController(IAsteriskStatusService asteriskStatusService)
        {
            _asteriskStatusService = asteriskStatusService;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            var status = _asteriskStatusService.GetStatus();
            var result = new
            {
                AsteriskStatus = status.Status
            };
            return Json(result);
        }
    }
}
