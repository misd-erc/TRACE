using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    public class NotificationManagementController : Controller
    {
        [Route("notifications")]
        public IActionResult Notifications()
        {
            return View();
        }
    }
}
