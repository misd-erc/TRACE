using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseEventTypeController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseEventTypeController(ErcdbContext context, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseEventTypes()
        {
            var categories = await _context.CaseEventTypes.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: CaseEventType
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseEventTypes.ToListAsync());
        }

        // GET: CaseEventType/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEventType = await _context.CaseEventTypes
                .FirstOrDefaultAsync(m => m.CaseEventTypeId == id);
            if (caseEventType == null)
            {
                return NotFound();
            }

            return View(caseEventType);
        }

        // GET: CaseEventType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseEventType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseEventTypeId,EventType")] CaseEventType caseEventType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseEventType);
                await _context.SaveChangesAsync();
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "CREATE";
                eventLog.Source = "ERC CASE EVENT ";
                eventLog.Category = "Case Event Type";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseEventType/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEventType = await _context.CaseEventTypes.FindAsync(id);
            if (caseEventType == null)
            {
                return NotFound();
            }
            return View(caseEventType);
        }

        // POST: CaseEventType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseEventTypeId,EventType")] CaseEventType caseEventType)
        {
            if (id != caseEventType.CaseEventTypeId)
            {
                 return Json(new { success = true, message = "Error! Missing CaseID" });
            }

            if (ModelState.IsValid)
            {
                _context.Update(caseEventType);
                await _context.SaveChangesAsync();
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "EDIT";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "Case Event Type";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been updated." });

            }
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseEventType/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEventType = await _context.CaseEventTypes
                .FirstOrDefaultAsync(m => m.CaseEventTypeId == id);
            if (caseEventType == null)
            {
                return NotFound();
            }

            return View(caseEventType);
        }

        // POST: CaseEventType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseEventType = await _context.CaseEventTypes.FindAsync(id);
            if (caseEventType != null)
            {
                _context.CaseEventTypes.Remove(caseEventType);
            }
            EventLog eventLog = new EventLog();
            eventLog.EventDatetime = DateTime.Now;
            var currentUserName = _currentUserHelper.Email;
            var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
            eventLog.UserId = user.Username;
            eventLog.Event = "DELETE";
            eventLog.Source = "CONTENT MANAGEMENT";
            eventLog.Category = "Case Event Type";
            _context.EventLogs.Add(eventLog);
            await _context.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));
        }

        private bool CaseEventTypeExists(long id)
        {
            return _context.CaseEventTypes.Any(e => e.CaseEventTypeId == id);
        }
    }
}
