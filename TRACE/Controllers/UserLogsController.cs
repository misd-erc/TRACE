using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    [Route("useraudit")]
    public class UserLogsController : Controller
    {
        public IActionResult UserLogs()
        {
            return View();
        }
    }
}
