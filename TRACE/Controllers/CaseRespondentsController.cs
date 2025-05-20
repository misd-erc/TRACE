using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class CaseRespondentsController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseRespondentsController(ErcdbContext context, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }
        public async Task<IActionResult> GetCaseRespondentByErcID(int id)
        {
            var caseRespondents = await _context.CaseRespondents
                .Where(x => x.ErccaseId == id)
                .Select(cr => new {
                    cr.CaseRespondentId,
                    cr.ErccaseId,
                    cr.CompanyId,
                    CompanyName = cr.Company != null ? cr.Company.CompanyName : null,
                    cr.CorrespondentId,
                    CorrespondentName = cr.Correspondent != null ? cr.Correspondent.FirstName + " " + cr.Correspondent.LastName : null
                    // Add only what you need here
                })
                .ToListAsync();

            return Json(caseRespondents);
        }
        // GET: CaseRespondents
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseRespondents.Include(c => c.Company).Include(c => c.Correspondent).Include(c => c.Erccase);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: CaseRespondents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseRespondent = await _context.CaseRespondents
                .Include(c => c.Company)
                .Include(c => c.Correspondent)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseRespondentId == id);
            if (caseRespondent == null)
            {
                return NotFound();
            }

            return View(caseRespondent);
        }

        // GET: CaseRespondents/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(
                _context.Companies
                    .OrderBy(c => c.CompanyName)
                    .Take(20),
                "CompanyId",
                "CompanyName"
            );

            ViewData["CorrespondentId"] = new SelectList(
                _context.Correspondents
                    .Select(c => new {
                        c.CorrespondentId,
                        FullName = c.Salutation + " " + c.FirstName + " " + c.LastName
                    })
                    .OrderBy(c => c.FullName)
                    .Take(20),
                "CorrespondentId",
                "FullName"
            );

            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }
        // POST: CaseRespondents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseRespondentId,ErccaseId,Remarks,CorrespondentId,CompanyId")] CaseRespondent caseRespondent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseRespondent);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "CREATE";
                eventLog.Source = "ERC CASE";
                eventLog.Category = "Create Case Respondents";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseRespondent.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseRespondent.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseRespondent.ErccaseId);
            return View(caseRespondent);
        }

        // GET: CaseRespondents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseRespondent = await _context.CaseRespondents.FindAsync(id);
            if (caseRespondent == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseRespondent.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseRespondent.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseRespondent.ErccaseId);
            return View(caseRespondent);
        }

        // POST: CaseRespondents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseRespondentId,ErccaseId,Remarks,CorrespondentId,CompanyId")] CaseRespondent caseRespondent)
        {
            if (id != caseRespondent.CaseRespondentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "ERC CASE";
                    eventLog.Category = "Create Case Respondents";
                    _context.EventLogs.Add(eventLog);
                    _context.Update(caseRespondent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseRespondentExists(caseRespondent.CaseRespondentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseRespondent.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseRespondent.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseRespondent.ErccaseId);
            return View(caseRespondent);
        }

        // GET: CaseRespondents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseRespondent = await _context.CaseRespondents
                .Include(c => c.Company)
                .Include(c => c.Correspondent)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseRespondentId == id);
            if (caseRespondent == null)
            {
                return NotFound();
            }

            return View(caseRespondent);
        }

        // POST: CaseRespondents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseRespondent = await _context.CaseRespondents.FindAsync(id);
            if (caseRespondent != null)
            {
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "ERC CASE";
                eventLog.Category = "Create Case Respondents";
                _context.EventLogs.Add(eventLog);
                _context.CaseRespondents.Remove(caseRespondent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseRespondentExists(long id)
        {
            return _context.CaseRespondents.Any(e => e.CaseRespondentId == id);
        }

        public IActionResult Search1(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var results = _context.Companies
                .Where(e => e.CompanyName.ToString().Contains(term))
                .OrderBy(e => e.CompanyName)
                .Take(15)
                .Select(e => new
                {
                    id = e.CompanyId,
                    text = e.CompanyName
                })
                .ToList();

            return Json(results);
        }

        public IActionResult Search2(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var results = _context.Correspondents
                .Where(e => (e.FirstName + " " + e.MiddleName + " " + e.LastName).Contains(term))
                .OrderBy(e => e.FirstName)
                .Take(15)
                .Select(e => new
                {
                    id = e.CorrespondentId,
                    text = e.FirstName + " " + e.MiddleName + " " + e.LastName
                })
                .ToList();

            return Json(results);
        }
    }
}
