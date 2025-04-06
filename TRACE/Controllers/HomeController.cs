using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CurrentUserHelper _currentUser;
        private readonly GenerateOTPHelper _generateOtp;
        private readonly GetGroupMemberHelper _getGroupMemberHelper;

        public HomeController(
            ILogger<HomeController> logger,
            CurrentUserHelper currentUser,
            GenerateOTPHelper generateOtp,
            GetGroupMemberHelper getGroupMemberHelper)
        {
            _logger = logger;
            _currentUser = currentUser;
            _generateOtp = generateOtp;
            _getGroupMemberHelper = getGroupMemberHelper;
        }

        [Route("auth")]
        public async Task<IActionResult> Authentication()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Logout", "External");
            }


            var groupEmails = await _getGroupMemberHelper.GetGroupMemberEmailsAsync();
            bool isAuthorized = groupEmails.Any(e => string.Equals(e, email, StringComparison.OrdinalIgnoreCase));

            if (!isAuthorized)
            {
                return RedirectToAction("ForbiddenAccess", "External");
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
