using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseManagementController : Controller
    {
        [Route("allcases")]
        public IActionResult AllCases()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("AllCases/AllCases");
        }

        [Route("lettercomplaints")]
        public IActionResult LetterComplaints()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("MyCases/LetterComplaints/LetterComplaints");
        }

        [Route("casemanagement/create")]
        public IActionResult CreateCase()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("CreateCase/CreateCase");
        }

        [Route("casedetails")]
        public IActionResult CaseDetails()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("CaseDetails/CaseDetails");
        }

        [Route("docketedcases")]
        public IActionResult DocketedCases()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("MyCases/DocketedCases/DocketedCases");
        }
    }
}