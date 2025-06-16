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
    public class DuinCaseController : Controller
    {
        private readonly ErcdbContext _context;

        public DuinCaseController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: DuinCase
        public async Task<IActionResult> Index()
        {
            return View(await _context.DuinCases.ToListAsync());
        }

        // GET: DuinCase/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duinCase = await _context.DuinCases
                .FirstOrDefaultAsync(m => m.DuinCasesId == id);
            if (duinCase == null)
            {
                return NotFound();
            }

            return View(duinCase);
        }

        // GET: DuinCase/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DuinCase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DuinCasesId,DuId,CaseId")] DuinCase duinCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(duinCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(duinCase);
        }

        // GET: DuinCase/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duinCase = await _context.DuinCases.FindAsync(id);
            if (duinCase == null)
            {
                return NotFound();
            }
            return View(duinCase);
        }

        // POST: DuinCase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DuinCasesId,DuId,CaseId")] DuinCase duinCase)
        {
            if (id != duinCase.DuinCasesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(duinCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuinCaseExists(duinCase.DuinCasesId))
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
            return View(duinCase);
        }

        // GET: DuinCase/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duinCase = await _context.DuinCases
                .FirstOrDefaultAsync(m => m.DuinCasesId == id);
            if (duinCase == null)
            {
                return NotFound();
            }

            return View(duinCase);
        }

        // POST: DuinCase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var duinCase = await _context.DuinCases.FindAsync(id);
            if (duinCase != null)
            {
                _context.DuinCases.Remove(duinCase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DuinCaseExists(long id)
        {
            return _context.DuinCases.Any(e => e.DuinCasesId == id);
        }
    }
}
