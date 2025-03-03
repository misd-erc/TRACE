using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[Authorize]
        [Route("auth")]
        public IActionResult authentication()
        {
            return View();
        }

        [Authorize]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {

            var name = User.FindFirst("name")?.Value;
            var email = User.FindFirst("preferred_username")?.Value;

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
