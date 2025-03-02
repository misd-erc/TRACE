using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Route("")]
    public class ExternalController : Controller
    {
        [Route("")]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
