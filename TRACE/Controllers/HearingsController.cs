using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    public class HearingsController : Controller
    {
        [Route("hearings")]
        public IActionResult Hearings()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View();
        }
    }
}
