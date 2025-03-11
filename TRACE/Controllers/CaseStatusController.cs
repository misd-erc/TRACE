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
    public class CaseStatusController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseStatusController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: CaseStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseStatuses.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseStatus()
        {
            var categories = await _context.CaseStatuses.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: CaseStatus/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseStatus = await _context.CaseStatuses
                .FirstOrDefaultAsync(m => m.CaseStatusId == id);
            if (caseStatus == null)
            {
                return NotFound();
            }

            return View(caseStatus);
        }

        // GET: CaseStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseStatusId,Status,Description")] CaseStatus caseStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseStatus);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseStatus/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseStatus = await _context.CaseStatuses.FindAsync(id);
            if (caseStatus == null)
            {
                return NotFound();
            }
            return View(caseStatus);
        }

        // POST: CaseStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseStatusId,Status,Description")] CaseStatus caseStatus)
        {
            if (id != caseStatus.CaseStatusId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseStatusExists(caseStatus.CaseStatusId))
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

        // GET: CaseStatus/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseStatus = await _context.CaseStatuses
                .FirstOrDefaultAsync(m => m.CaseStatusId == id);
            if (caseStatus == null)
            {
                return NotFound();
            }

            return View(caseStatus);
        }

        // POST: CaseStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseStatus = await _context.CaseStatuses.FindAsync(id);
            if (caseStatus != null)
            {
                _context.CaseStatuses.Remove(caseStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseStatusExists(long id)
        {
            return _context.CaseStatuses.Any(e => e.CaseStatusId == id);
        }
    }
}
