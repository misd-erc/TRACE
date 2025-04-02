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
    public class CaseNoteController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseNoteController(ErcdbContext context, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        // GET: CaseNote
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseNotes.Include(c => c.Erccase);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseNoteByErcID(int id)
        {
            var categories = await _context.CaseNotes.Where(x => x.ErccaseId == id).ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: CaseNote/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNote = await _context.CaseNotes
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseNoteId == id);
            if (caseNote == null)
            {
                return NotFound();
            }

            return View(caseNote);
        }

        // GET: CaseNote/Create
        public IActionResult Create()
        {
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: CaseNote/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseNoteId,Notes,ErccaseId,DatetimeCreated,NotedBy")] CaseNote caseNote)
        {
            if (!ModelState.IsValid)

            {
                caseNote.DatetimeCreated = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                caseNote.NotedBy = user.Username;
                _context.Add(caseNote);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseNote.ErccaseId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseNote/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNote = await _context.CaseNotes.FindAsync(id);
            if (caseNote == null)
            {
                return NotFound();
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseNote.ErccaseId);
            return View(caseNote);
        }

        // POST: CaseNote/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseNoteId,Notes,ErccaseId,DatetimeCreated,NotedBy")] CaseNote caseNote)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                return Json(new { success = false, message = "Error! Invalid input.", errors });
            }

            var existingCaseNote = await _context.CaseNotes.FindAsync(id);
            if (existingCaseNote == null)
            {
                return Json(new { success = false, message = "Error! Case note not found." });
            }

            try
            {
                // Update only the necessary fields
                existingCaseNote.Notes = caseNote.Notes;
                existingCaseNote.ErccaseId = caseNote.ErccaseId;
                existingCaseNote.DatetimeCreated = caseNote.DatetimeCreated;
                existingCaseNote.NotedBy = caseNote.NotedBy;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, message = "Error! Failed to save changes." });
            }
        }


        // GET: CaseNote/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNote = await _context.CaseNotes
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseNoteId == id);
            if (caseNote == null)
            {
                return NotFound();
            }

            return View(caseNote);
        }

        // POST: CaseNote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseNote = await _context.CaseNotes.FindAsync(id);
            if (caseNote != null)
            {
                _context.CaseNotes.Remove(caseNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseNoteExists(long id)
        {
            return _context.CaseNotes.Any(e => e.CaseNoteId == id);
        }
    }
}
