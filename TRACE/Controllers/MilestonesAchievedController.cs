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
    public class MilestonesAchievedController : Controller
    {
        private readonly ErcdbContext _context;

        public MilestonesAchievedController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: MilestonesAchieved
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.MilestonesAchieveds.Include(m => m.CaseMilestone).Include(m => m.Erccase);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: MilestonesAchieved/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestonesAchieved = await _context.MilestonesAchieveds
                .Include(m => m.CaseMilestone)
                .Include(m => m.Erccase)
                .FirstOrDefaultAsync(m => m.MilestoneAchievedId == id);
            if (milestonesAchieved == null)
            {
                return NotFound();
            }

            return View(milestonesAchieved);
        }

        // GET: MilestonesAchieved/Create
        public IActionResult Create()
        {
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId");
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: MilestonesAchieved/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MilestoneAchievedId,ErccaseId,CaseMilestoneId,DatetimeAchieved,PercentAchieved")] MilestonesAchieved milestonesAchieved)
        {
            if (ModelState.IsValid)
            {
                _context.Add(milestonesAchieved);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", milestonesAchieved.CaseMilestoneId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", milestonesAchieved.ErccaseId);
            return View(milestonesAchieved);
        }

        // GET: MilestonesAchieved/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestonesAchieved = await _context.MilestonesAchieveds.FindAsync(id);
            if (milestonesAchieved == null)
            {
                return NotFound();
            }
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", milestonesAchieved.CaseMilestoneId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", milestonesAchieved.ErccaseId);
            return View(milestonesAchieved);
        }

        // POST: MilestonesAchieved/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("MilestoneAchievedId,ErccaseId,CaseMilestoneId,DatetimeAchieved,PercentAchieved")] MilestonesAchieved milestonesAchieved)
        {
            if (id != milestonesAchieved.MilestoneAchievedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(milestonesAchieved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilestonesAchievedExists(milestonesAchieved.MilestoneAchievedId))
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
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", milestonesAchieved.CaseMilestoneId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", milestonesAchieved.ErccaseId);
            return View(milestonesAchieved);
        }

        // GET: MilestonesAchieved/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestonesAchieved = await _context.MilestonesAchieveds
                .Include(m => m.CaseMilestone)
                .Include(m => m.Erccase)
                .FirstOrDefaultAsync(m => m.MilestoneAchievedId == id);
            if (milestonesAchieved == null)
            {
                return NotFound();
            }

            return View(milestonesAchieved);
        }

        // POST: MilestonesAchieved/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var milestonesAchieved = await _context.MilestonesAchieveds.FindAsync(id);
            if (milestonesAchieved != null)
            {
                _context.MilestonesAchieveds.Remove(milestonesAchieved);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MilestonesAchievedExists(long id)
        {
            return _context.MilestonesAchieveds.Any(e => e.MilestoneAchievedId == id);
        }
    }
}
