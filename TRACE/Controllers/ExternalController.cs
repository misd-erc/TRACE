using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
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

        [Route("Logout")]
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties { RedirectUri = "/" },
                OpenIdConnectDefaults.AuthenticationScheme,
                "Cookies");
        }

        [Route("RedirectToMicrosoftLogin")]
        public IActionResult RedirectToMicrosoftLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("authentication", "Home")
            };

            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Route("accessdenied")]
        public IActionResult ForbiddenAccess()
        {
            return View();
        }

    }
}
