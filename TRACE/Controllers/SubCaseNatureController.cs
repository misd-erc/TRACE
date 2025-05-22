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
    public class SubCaseNatureController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public SubCaseNatureController(ErcdbContext context, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        // GET: SubCaseNature
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.SubCaseNature.Include(s => s.CaseNature);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetSubCaseNature()
        {
            var categories = await _context.SubCaseNature
                .Include(s => s.CaseNature)
                .Select(s => new {
                    s.SubNatureId,
                    s.SubNatureName,
                    s.Description,
                    CaseNatureName = s.CaseNature.Nature // Replace with your actual property
                })
                .ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: SubCaseNature/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCaseNature = await _context.SubCaseNature
                .Include(s => s.CaseNature)
                .FirstOrDefaultAsync(m => m.SubNatureId == id);
            if (subCaseNature == null)
            {
                return NotFound();
            }

            return View(subCaseNature);
        }

        // GET: SubCaseNature/Create
        public IActionResult Create()
        {
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "Nature");
            return View();
        }

        // POST: SubCaseNature/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubNatureId,SubNatureName,Description,CaseNatureId,IsInternal,CreatedAt")] SubCaseNature subCaseNature)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(subCaseNature);
                await _context.SaveChangesAsync();

                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                EventLog eventLog = new EventLog
                {
                    EventDatetime = DateTime.Now,
                    UserId = user?.Username,
                    Event = "CREATE",
                    Source = "CONTENT MANAGEMENT",
                    Category = "SubCase Nature"
                };

                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Sub Case Nature successfully created." });
            }

            return Json(new { success = false, message = "Validation failed. Please check your inputs." });
        }


        // GET: SubCaseNature/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCaseNature = await _context.SubCaseNature.FindAsync(id);
            if (subCaseNature == null)
            {
                return NotFound();
            }
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "CaseNatureId", subCaseNature.CaseNatureId);
            return View(subCaseNature);
        }

        // POST: SubCaseNature/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubNatureId,SubNatureName,Description,CaseNatureId,IsInternal,CreatedAt")] SubCaseNature subCaseNature)
        {
            if (id != subCaseNature.SubNatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCaseNature);
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "CONTENT MANAGEMENT";
                    eventLog.Category = "SubCase Nature";
                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCaseNatureExists(subCaseNature.SubNatureId))
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
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "CaseNatureId", subCaseNature.CaseNatureId);
            return View(subCaseNature);
        }

        // GET: SubCaseNature/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCaseNature = await _context.SubCaseNature
                .Include(s => s.CaseNature)
                .FirstOrDefaultAsync(m => m.SubNatureId == id);
            if (subCaseNature == null)
            {
                return NotFound();
            }

            return View(subCaseNature);
        }

        // POST: SubCaseNature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCaseNature = await _context.SubCaseNature.FindAsync(id);
            if (subCaseNature != null)
            {
                _context.SubCaseNature.Remove(subCaseNature);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "SubCase Nature";
                _context.EventLogs.Add(eventLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCaseNatureExists(int id)
        {
            return _context.SubCaseNature.Any(e => e.SubNatureId == id);
        }
    }
}
