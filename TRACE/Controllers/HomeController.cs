using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CurrentUserHelper _currentUser;
        private readonly GenerateOTPHelper _generateOtp;

        public HomeController(ILogger<HomeController> logger, CurrentUserHelper currentUser, GenerateOTPHelper generateOtp)
        {
            _logger = logger;
            _currentUser = currentUser;
            _generateOtp = generateOtp;
        }

        [Authorize]
        [Route("auth")]
        public IActionResult authentication()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Home");
            }

            var otp = _generateOtp.GenerateOtp();
            HttpContext.Session.SetString("UserOTP", otp);
            HttpContext.Session.SetString("UserEmail", email);

            var otpExpiry = DateTime.UtcNow.AddMinutes(5);
            HttpContext.Session.SetString("OtpExpiry", otpExpiry.ToString("o"));

            _generateOtp.SendOtpEmail(email, otp);

            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [Route("verify-otp")]
        public IActionResult VerifyOtp(string otp)
        {
            var storedOtp = HttpContext.Session.GetString("UserOTP");
            var otpExpiryString = HttpContext.Session.GetString("OtpExpiry");

            if (string.IsNullOrEmpty(storedOtp) || string.IsNullOrEmpty(otpExpiryString))
            {
                ViewBag.Error = "OTP has expired. Please request a new OTP.";
                return RedirectToAction("authentication");
            }

            var otpExpiry = DateTime.Parse(otpExpiryString, null, System.Globalization.DateTimeStyles.RoundtripKind);

            if (DateTime.UtcNow > otpExpiry)
            {
                ViewBag.Error = "OTP has expired. Please request a new OTP.";
                return RedirectToAction("authentication");
            }

            if (otp == storedOtp)
            {
                HttpContext.Session.SetString("IsVerified", "true");
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Error = "Invalid OTP. Please try again.";
                return View("authentication");
            }
        }

        [Authorize]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }

            var name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            ViewBag.Name = name;
            return View();
        }

        [Authorize]
        [Route("cases")]
        public IActionResult CaseManagement()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View();
        }

        [Authorize]
        [Route("documents")]
        public IActionResult DocumentManagement()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View();
        }

        [Authorize]
        [Route("hearings")]
        public IActionResult Hearings()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View();
        }

        [Authorize]
        [Route("settings")]
        public IActionResult Settings()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
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
