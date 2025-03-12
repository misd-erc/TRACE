using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    public class ContentManagementController : Controller
    {
        
        [Route("contentmanagement")]
        public IActionResult ContentManagement()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View();
        }
    }
}
