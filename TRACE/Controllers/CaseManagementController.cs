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

        [Route("letterofcomplaints")]
        public IActionResult LetterOfComplaints()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("MyCases/LetterOfComplaints/LetterOfComplaints");
        }

        [Route("letterofcomplaints/create")]
        public IActionResult CreateLetterOfComplaints()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("MyCases/LetterOfComplaints/CreateCase");
        }

        [Route("letterofcomplaints/casedetails")]
        public IActionResult CaseDetails()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("MyCases/LetterOfComplaints/CaseDetails");
        }

        [Route("dockettedcases")]
        public IActionResult DockettedCases()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View("MyCases/DockettedCases/DockettedCases");
        }
    }
}