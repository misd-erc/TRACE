using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class HearingCategoryController : Controller
    {
        private readonly ErcdbContext _context;

        public HearingCategoryController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: HearingCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.HearingCategories.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetHearingCategories()
        {
            var categories = await _context.HearingCategories.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: HearingCategory/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingCategory = await _context.HearingCategories
                .FirstOrDefaultAsync(m => m.HearingCategoryId == id);
            if (hearingCategory == null)
            {
                return NotFound();
            }

            return View(hearingCategory);
        }

        // GET: HearingCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HearingCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HearingCategoryId,Category,Description")] HearingCategory hearingCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hearingCategory);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: HearingCategory/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingCategory = await _context.HearingCategories.FindAsync(id);
            if (hearingCategory == null)
            {
                return NotFound();
            }
            return View(hearingCategory);
        }

        // POST: HearingCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HearingCategoryId,Category,Description")] HearingCategory hearingCategory)
        {
            if (id != hearingCategory.HearingCategoryId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hearingCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HearingCategoryExists(hearingCategory.HearingCategoryId))
                    {
                        return Json(new { success = false, message = "Error! Data not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true, message = "Success! Data has been updated." });
            }
            return Json(new { success = true, message = "Success! Data has been updated." });
        }

        // GET: HearingCategory/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingCategory = await _context.HearingCategories
                .FirstOrDefaultAsync(m => m.HearingCategoryId == id);
            if (hearingCategory == null)
            {
                return NotFound();
            }

            return View(hearingCategory);
        }

        // POST: HearingCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hearingCategory = await _context.HearingCategories.FindAsync(id);
            if (hearingCategory != null)
            {
                _context.HearingCategories.Remove(hearingCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HearingCategoryExists(long id)
        {
            return _context.HearingCategories.Any(e => e.HearingCategoryId == id);
        }
    }
}
