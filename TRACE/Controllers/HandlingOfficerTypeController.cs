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
    public class HandlingOfficerTypeController : Controller
    {
        private readonly ErcdbContext _context;

        public HandlingOfficerTypeController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: HearingOfficerType
        public async Task<IActionResult> Index()
        {
            return View(await _context.HearingOfficerTypes.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetHearingOfficerTypes()
        {
            var categories = await _context.HearingOfficerTypes.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: HearingOfficerType/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingOfficerType = await _context.HearingOfficerTypes
                .FirstOrDefaultAsync(m => m.HearingOfficerTypeId == id);
            if (hearingOfficerType == null)
            {
                return NotFound();
            }

            return View(hearingOfficerType);
        }

        // GET: HearingOfficerType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HearingOfficerType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HearingOfficerTypeId,OfficerType")] HearingOfficerType hearingOfficerType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hearingOfficerType);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: HearingOfficerType/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingOfficerType = await _context.HearingOfficerTypes.FindAsync(id);
            if (hearingOfficerType == null)
            {
                return NotFound();
            }
            return View(hearingOfficerType);
        }

        // POST: HearingOfficerType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HearingOfficerTypeId,OfficerType")] HearingOfficerType hearingOfficerType)
        {
            if (id != hearingOfficerType.HearingOfficerTypeId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hearingOfficerType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HearingOfficerTypeExists(hearingOfficerType.HearingOfficerTypeId))
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

        // GET: HearingOfficerType/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingOfficerType = await _context.HearingOfficerTypes
                .FirstOrDefaultAsync(m => m.HearingOfficerTypeId == id);
            if (hearingOfficerType == null)
            {
                return NotFound();
            }

            return View(hearingOfficerType);
        }

        // POST: HearingOfficerType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hearingOfficerType = await _context.HearingOfficerTypes.FindAsync(id);
            if (hearingOfficerType != null)
            {
                _context.HearingOfficerTypes.Remove(hearingOfficerType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HearingOfficerTypeExists(long id)
        {
            return _context.HearingOfficerTypes.Any(e => e.HearingOfficerTypeId == id);
        }
    }
}
