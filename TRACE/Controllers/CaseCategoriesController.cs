using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseCategoriesController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseCategoriesController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: CaseCategories

        public async Task<IActionResult> Index()
        {
            var categories = await _context.CaseCategories.ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseCategories()
        {
            var categories = await _context.CaseCategories.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: CaseCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseCategory = await _context.CaseCategories
                .FirstOrDefaultAsync(m => m.CaseCategoryId == id);
            if (caseCategory == null)
            {
                return NotFound();
            }

            return View(caseCategory);
        }

        // GET: CaseCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseCategoryId,Category,Description,IsInternal")] CaseCategory caseCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseCategory);
        }

        // GET: CaseCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseCategory = await _context.CaseCategories.FindAsync(id);
            if (caseCategory == null)
            {
                return NotFound();
            }
            return View(caseCategory);
        }

        // POST: CaseCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseCategoryId,Category,Description,IsInternal")] CaseCategory caseCategory)
        {
            if (id != caseCategory.CaseCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseCategoryExists(caseCategory.CaseCategoryId))
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
            return View(caseCategory);
        }

        // GET: CaseCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseCategory = await _context.CaseCategories
                .FirstOrDefaultAsync(m => m.CaseCategoryId == id);
            if (caseCategory == null)
            {
                return NotFound();
            }

            return View(caseCategory);
        }

        // POST: CaseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseCategory = await _context.CaseCategories.FindAsync(id);
            if (caseCategory != null)
            {
                _context.CaseCategories.Remove(caseCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseCategoryExists(long id)
        {
            return _context.CaseCategories.Any(e => e.CaseCategoryId == id);
        }
    }
}
