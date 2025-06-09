using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        [Route("casereports")]
        public IActionResult GenerateReports()
        {
            return View();
        }
    }
}
