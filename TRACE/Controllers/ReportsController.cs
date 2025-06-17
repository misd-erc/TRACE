using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Helpers;

namespace TRACE.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly EventLogger _eventLogger;
        private readonly CurrentUserHelper _currentUserHelper;

        public ReportsController(ErcdbContext context, IConfiguration configuration, EventLogger eventLogger, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _eventLogger = eventLogger;
            _currentUserHelper = currentUserHelper;
        }

        [Route("casereports")]
        public IActionResult GenerateReports()
        {
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description");
            ViewData["ErccaseId"] = new SelectList(_context.Erccases.Take(20), "ErccaseId", "CaseNo");
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "Milestone");
            ViewData["CorrespondentId"] = new SelectList(
                _context.Correspondents
                .Select(c => new {
                    c.CorrespondentId,
                    FullName = c.Salutation + " " + c.FirstName + " " + c.LastName
                })
                .Take(20),
                "CorrespondentId",
                "FullName"
                );
            return View();
        }

        [Route("casereports/Search")]
        public IActionResult Search(string category, string term)
        {
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());
            switch (category.ToLower())
            {
                case "case":
                    var cases = _context.Erccases.Where(e => e.CaseNo.ToString().Contains(term))
                    .OrderBy(e => e.ErccaseId)
                    .Take(15)
                    .Select(e => new { id = e.ErccaseId, text = e.CaseNo })
                    .ToList();
                    return Json(cases);
                case "milestone":
                    var milestones = _context.CaseMilestones.Where(e => e.Milestone.ToString().Contains(term))
                        .OrderBy(e => e.CaseMilestoneId)
                        .Take(15)
                        .Select(e => new { id = e.CaseMilestoneId, text = e.Milestone })
                        .ToList();
                    return Json(milestones);
                case "respondent":
                    var respondents = _context.Correspondents.Where(e => (
                        e.Salutation!.Contains(term) || 
                        e.FirstName.Contains(term) || 
                        e.LastName.Contains(term))
                    )
                      .OrderBy(e => e.CorrespondentId)
                        .Take(15)
                        .Select(e => new { id = e.CorrespondentId, text = e.Salutation + " " + e.FirstName + " " + e.LastName })
                        .ToList();
                    return Json(respondents);
                default:
                    break;
            }
            return Json(new List<object>());
        }
    }
}
