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
    public class CaseMilestoneController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseMilestoneController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: CaseMilestone
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseMilestones.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseMilestones()
        {
            var categories = await _context.CaseMilestones.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: CaseMilestone/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestone = await _context.CaseMilestones
                .FirstOrDefaultAsync(m => m.CaseMilestoneId == id);
            if (caseMilestone == null)
            {
                return NotFound();
            }

            return View(caseMilestone);
        }

        // GET: CaseMilestone/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseMilestone/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseMilestoneId,Milestone,Description")] CaseMilestone caseMilestone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseMilestone);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseMilestone/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestone = await _context.CaseMilestones.FindAsync(id);
            if (caseMilestone == null)
            {
                return NotFound();
            }
            return View(caseMilestone);
        }

        // POST: CaseMilestone/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseMilestoneId,Milestone,Description")] CaseMilestone caseMilestone)
        {
            if (id != caseMilestone.CaseMilestoneId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseMilestone);
                    await _context.SaveChangesAsync();
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseMilestoneExists(caseMilestone.CaseMilestoneId))
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

        // GET: CaseMilestone/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestone = await _context.CaseMilestones
                .FirstOrDefaultAsync(m => m.CaseMilestoneId == id);
            if (caseMilestone == null)
            {
                return NotFound();
            }

            return View(caseMilestone);
        }

        // POST: CaseMilestone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseMilestone = await _context.CaseMilestones.FindAsync(id);
            if (caseMilestone != null)
            {
                _context.CaseMilestones.Remove(caseMilestone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseMilestoneExists(long id)
        {
            return _context.CaseMilestones.Any(e => e.CaseMilestoneId == id);
        }
    }
}
