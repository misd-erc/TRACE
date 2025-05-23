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
    public class CaseMilestoneTemplateController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseMilestoneTemplateController(ErcdbContext context, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        // GET: CaseMilestoneTemplate
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseMilestoneTemplates.Include(c => c.CaseCategory);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseMilestoneTemplate()
        {
            var templates = await _context.CaseMilestoneTemplates
                .Include(c => c.CaseCategory)
                .Select(t => new {
                    t.CaseMilestoneTemplateId,
                    t.TemplateName,
                    CaseCategoryName = t.CaseCategory.Category
                })
                .ToListAsync();

            if (templates == null || !templates.Any())
            {
                return Json(new { success = false, message = "No data found." });
            }

            return Json(new { success = true, data = templates });
        }

        // GET: CaseMilestoneTemplate/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestoneTemplate = await _context.CaseMilestoneTemplates
                .Include(c => c.CaseCategory)
                .FirstOrDefaultAsync(m => m.CaseMilestoneTemplateId == id);
            if (caseMilestoneTemplate == null)
            {
                return NotFound();
            }

            return View(caseMilestoneTemplate);
        }

        // GET: CaseMilestoneTemplate/Create
        public IActionResult Create()
        {
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description");
            return View();
        }

        // POST: CaseMilestoneTemplate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseMilestoneTemplateId,TemplateName,CaseCategoryId")] CaseMilestoneTemplate caseMilestoneTemplate)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(caseMilestoneTemplate);

                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                EventLog eventLog = new EventLog
                {
                    EventDatetime = DateTime.Now,
                    UserId = user?.Username ?? "Unknown",
                    Event = "CREATE",
                    Source = "CONTENT MANAGEMENT",
                    Category = "Case Milestone Template"
                };
                _context.EventLogs.Add(eventLog);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Milestone Template created successfully!" });
            }

            // Extract validation error messages
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join(" ", errors) });
        }

        // GET: CaseMilestoneTemplate/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestoneTemplate = await _context.CaseMilestoneTemplates.FindAsync(id);
            if (caseMilestoneTemplate == null)
            {
                return NotFound();
            }
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description", caseMilestoneTemplate.CaseCategoryId);
            return View(caseMilestoneTemplate);
        }

        // POST: CaseMilestoneTemplate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseMilestoneTemplateId,TemplateName,CaseCategoryId")] CaseMilestoneTemplate caseMilestoneTemplate)
        {
            if (id != caseMilestoneTemplate.CaseMilestoneTemplateId)
            {
                return Json(new { success = false, message = "Invalid template ID." });
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseMilestoneTemplate);

                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                    EventLog eventLog = new EventLog
                    {
                        EventDatetime = DateTime.Now,
                        UserId = user?.Username ?? "Unknown",
                        Event = "EDIT",
                        Source = "CONTENT MANAGEMENT",
                        Category = "Case Milestone Template"
                    };
                    _context.EventLogs.Add(eventLog);

                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Milestone Template updated successfully!" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseMilestoneTemplateExists(caseMilestoneTemplate.CaseMilestoneTemplateId))
                    {
                        return Json(new { success = false, message = "Milestone Template not found." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "A concurrency error occurred. Please try again." });
                    }
                }
            }

            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = string.Join(" ", errors) });
        }


        // GET: CaseMilestoneTemplate/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestoneTemplate = await _context.CaseMilestoneTemplates
                .Include(c => c.CaseCategory)
                .FirstOrDefaultAsync(m => m.CaseMilestoneTemplateId == id);
            if (caseMilestoneTemplate == null)
            {
                return NotFound();
            }

            return View(caseMilestoneTemplate);
        }

        // POST: CaseMilestoneTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseMilestoneTemplate = await _context.CaseMilestoneTemplates.FindAsync(id);
            if (caseMilestoneTemplate != null)
            {
                _context.CaseMilestoneTemplates.Remove(caseMilestoneTemplate);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "Case Milestone Template";
                _context.EventLogs.Add(eventLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseMilestoneTemplateExists(long id)
        {
            return _context.CaseMilestoneTemplates.Any(e => e.CaseMilestoneTemplateId == id);
        }
    }
}
