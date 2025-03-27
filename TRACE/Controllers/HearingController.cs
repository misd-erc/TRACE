using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class HearingController : Controller
    {
        private readonly ErcdbContext _context;

        public HearingController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: Hearing
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.Hearings.Include(h => h.Erccase).Include(h => h.HearingCategory).Include(h => h.HearingVenue);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetHearingByErcID(int id)
        {
            var categories = await _context.Hearings.Where(x => x.ErccaseId == id).ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: Hearing/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearing = await _context.Hearings
                .Include(h => h.Erccase)
                .Include(h => h.HearingCategory)
                .Include(h => h.HearingVenue)
                .FirstOrDefaultAsync(m => m.HearingId == id);
            if (hearing == null)
            {
                return NotFound();
            }

            return View(hearing);
        }

        // GET: Hearing/Create
        public IActionResult Create()
        {
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId");
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId");
            return View();
        }

        // POST: Hearing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HearingId,ErccaseId,HearingDate,Time,HearingVenueId,Remarks,HearingCategoryId,IsApproved,ApprovedBy,DatetimeApproved,OtherVenue")] Hearing hearing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hearing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);
            return View(hearing);
        }

        // GET: Hearing/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearing = await _context.Hearings.FindAsync(id);
            if (hearing == null)
            {
                return NotFound();
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);
            return View(hearing);
        }

        // POST: Hearing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HearingId,ErccaseId,HearingDate,Time,HearingVenueId,Remarks,HearingCategoryId,IsApproved,ApprovedBy,DatetimeApproved,OtherVenue")] Hearing hearing)
        {
            if (id != hearing.HearingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hearing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HearingExists(hearing.HearingId))
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
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);
            return View(hearing);
        }

        // GET: Hearing/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearing = await _context.Hearings
                .Include(h => h.Erccase)
                .Include(h => h.HearingCategory)
                .Include(h => h.HearingVenue)
                .FirstOrDefaultAsync(m => m.HearingId == id);
            if (hearing == null)
            {
                return NotFound();
            }

            return View(hearing);
        }

        // POST: Hearing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hearing = await _context.Hearings.FindAsync(id);
            if (hearing != null)
            {
                _context.Hearings.Remove(hearing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HearingExists(long id)
        {
            return _context.Hearings.Any(e => e.HearingId == id);
        }
    }
}
