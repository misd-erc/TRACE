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
            var categories = await _context.CaseMilestoneTemplates.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
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
            if (ModelState.IsValid)
            {
                _context.Add(caseMilestoneTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "CaseCategoryId", caseMilestoneTemplate.CaseCategoryId);
            return View(caseMilestoneTemplate);
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
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseMilestoneTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseMilestoneTemplateExists(caseMilestoneTemplate.CaseMilestoneTemplateId))
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
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "CaseCategoryId", caseMilestoneTemplate.CaseCategoryId);
            return View(caseMilestoneTemplate);
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
