using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CurrentUserHelper _currentUser;

        public HomeController(ILogger<HomeController> logger, CurrentUserHelper currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        [Authorize]
        [Route("auth")]
        public IActionResult authentication()
        {
            return View();
        }

        [Authorize]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            var name = _currentUser.Name;
            var email = _currentUser.Email;

            ViewBag.Name = name;
            ViewBag.Email = email;

            return View();
        }

        [Authorize]
        [Route("cases")]
        public IActionResult CaseManagement()
        {
            return View();
        }

        [Authorize]
        [Route("documents")]
        public IActionResult DocumentManagement()
        {
            return View();
        }

        [Authorize]
        [Route("hearings")]
        public IActionResult Hearings()
        {
            return View();
        }

        [Authorize]
        [Route("settings")]
        public IActionResult Settings()
        {
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
