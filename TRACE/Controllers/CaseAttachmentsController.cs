using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseAttachmentsController : Controller
    {
        [Route("caseattachments/upload")]
        public IActionResult CaseAttachments()
        {
            return View();
        }
    }
}
