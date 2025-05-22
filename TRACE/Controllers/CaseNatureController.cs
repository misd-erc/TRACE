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
    public class CaseNatureController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseNatureController(ErcdbContext context, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        // GET: CaseNature
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseNatures.Include(c => c.CaseCategory);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCaseNature()
        {
            var categories = await _context.CaseNatures.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: CaseNature/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNature = await _context.CaseNatures
                .Include(c => c.CaseCategory)
                .FirstOrDefaultAsync(m => m.CaseNatureId == id);
            if (caseNature == null)
            {
                return NotFound();
            }

            return View(caseNature);
        }

        // GET: CaseNature/Create
        public IActionResult Create()
        {
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description");
            return View();
        }

        // POST: CaseNature/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseNatureId,Nature,Description,CaseCategoryId,Active")] CaseNature caseNature)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(caseNature);

                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                EventLog eventLog = new EventLog
                {
                    EventDatetime = DateTime.Now,
                    UserId = user?.Username,
                    Event = "CREATE",
                    Source = "CONTENT MANAGEMENT",
                    Category = "Case Nature"
                };

                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Case Nature created successfully." });
            }

            // Gather model validation errors
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = "Validation failed.", errors = errorMessages });
        }


        // GET: CaseNature/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNature = await _context.CaseNatures.FindAsync(id);
            if (caseNature == null)
            {
                return NotFound();
            }
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description", caseNature.CaseCategoryId);
            return View(caseNature);
        }

        // POST: CaseNature/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseNatureId,Nature,Description,CaseCategoryId,Active")] CaseNature caseNature)
        {
            if (id != caseNature.CaseNatureId)
            {
                return Json(new { success = false, message = "Invalid request. ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseNature);

                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                    EventLog eventLog = new EventLog
                    {
                        EventDatetime = DateTime.Now,
                        UserId = user?.Username,
                        Event = "EDIT",
                        Source = "CONTENT MANAGEMENT",
                        Category = "Case Nature"
                    };

                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Case Nature updated successfully." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseNatureExists(caseNature.CaseNatureId))
                    {
                        return Json(new { success = false, message = "Case Nature no longer exists." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "A concurrency error occurred." });
                    }
                }
            }

            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = "Validation failed.", errors });
        }


        // GET: CaseNature/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseNature = await _context.CaseNatures
                .Include(c => c.CaseCategory)
                .FirstOrDefaultAsync(m => m.CaseNatureId == id);
            if (caseNature == null)
            {
                return NotFound();
            }

            return View(caseNature);
        }

        // POST: CaseNature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseNature = await _context.CaseNatures.FindAsync(id);
            if (caseNature != null)
            {
                _context.CaseNatures.Remove(caseNature);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "Case Nature";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseNatureExists(long id)
        {
            return _context.CaseNatures.Any(e => e.CaseNatureId == id);
        }
    }
}
