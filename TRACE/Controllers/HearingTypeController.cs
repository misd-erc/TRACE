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
    public class HearingTypeController : Controller
    {
        private readonly ErcdbContext _context;

        public HearingTypeController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: HearingType
        public async Task<IActionResult> Index()
        {
            return View(await _context.HearingTypes.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetHearingTypes()
        {
            var categories = await _context.HearingTypes.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: HearingType/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingType = await _context.HearingTypes
                .FirstOrDefaultAsync(m => m.HearingTypeId == id);
            if (hearingType == null)
            {
                return NotFound();
            }

            return View(hearingType);
        }

        // GET: HearingType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HearingType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HearingTypeId,TypeOfHearing,Description")] HearingType hearingType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hearingType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: HearingType/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingType = await _context.HearingTypes.FindAsync(id);
            if (hearingType == null)
            {
                return NotFound();
            }
            return View(hearingType);
        }

        // POST: HearingType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HearingTypeId,TypeOfHearing,Description")] HearingType hearingType)
        {
            if (id != hearingType.HearingTypeId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hearingType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HearingTypeExists(hearingType.HearingTypeId))
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

        // GET: HearingType/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingType = await _context.HearingTypes
                .FirstOrDefaultAsync(m => m.HearingTypeId == id);
            if (hearingType == null)
            {
                return NotFound();
            }

            return View(hearingType);
        }

        // POST: HearingType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hearingType = await _context.HearingTypes.FindAsync(id);
            if (hearingType != null)
            {
                _context.HearingTypes.Remove(hearingType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HearingTypeExists(long id)
        {
            return _context.HearingTypes.Any(e => e.HearingTypeId == id);
        }
    }
}
