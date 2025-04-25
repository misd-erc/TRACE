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
    public class CaseEventController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseEventController(ErcdbContext context, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        // GET: CaseEvent
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseEvents.Include(c => c.CaseEventType).Include(c => c.Erccase);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseEventByErcID(int id)
        {
            var categories = await _context.CaseEvents.Where(x=>x.ErccaseId == id).ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: CaseEvent/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEvent = await _context.CaseEvents
                .Include(c => c.CaseEventType)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseEventId == id);
            if (caseEvent == null)
            {
                return NotFound();
            }

            return View(caseEvent);
        }

        // GET: CaseEvent/Create
        public IActionResult Create()
        {
            ViewData["CaseEventTypeId"] = new SelectList(_context.CaseEventTypes, "CaseEventTypeId", "EventType");
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: CaseEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseEventId,ErccaseId,EventDatetime,UserId,EventDescription,IsUserAction,CaseEventTypeId")] CaseEvent caseEvent)
        {
            if (!ModelState.IsValid)
            {
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                caseEvent.UserId = user.Username;
                _context.Add(caseEvent);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            ViewData["CaseEventTypeId"] = new SelectList(_context.CaseEventTypes, "CaseEventTypeId", "CaseEventTypeId", caseEvent.CaseEventTypeId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseEvent.ErccaseId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseEvent/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEvent = await _context.CaseEvents.FindAsync(id);
            if (caseEvent == null)
            {
                return NotFound();
            }
            ViewData["CaseEventTypeId"] = new SelectList(_context.CaseEventTypes, "CaseEventTypeId", "CaseEventTypeId", caseEvent.CaseEventTypeId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseEvent.ErccaseId);
            return View(caseEvent);
        }

        // POST: CaseEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseEventId,ErccaseId,EventDatetime,UserId,EventDescription,IsUserAction,CaseEventTypeId")] CaseEvent caseEvent)
        {
            if (id != caseEvent.CaseEventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseEventExists(caseEvent.CaseEventId))
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
            ViewData["CaseEventTypeId"] = new SelectList(_context.CaseEventTypes, "CaseEventTypeId", "CaseEventTypeId", caseEvent.CaseEventTypeId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseEvent.ErccaseId);
            return View(caseEvent);
        }

        // GET: CaseEvent/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEvent = await _context.CaseEvents
                .Include(c => c.CaseEventType)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseEventId == id);
            if (caseEvent == null)
            {
                return NotFound();
            }

            return View(caseEvent);
        }

        // POST: CaseEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseEvent = await _context.CaseEvents.FindAsync(id);
            if (caseEvent != null)
            {
                _context.CaseEvents.Remove(caseEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseEventExists(long id)
        {
            return _context.CaseEvents.Any(e => e.CaseEventId == id);
        }
    }
}
