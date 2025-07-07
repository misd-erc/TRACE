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
    public class IntervenorController : Controller
    {
        private readonly ErcdbContext _context;

        public IntervenorController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: Intervenor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Intervenors.ToListAsync());
        }

        // GET: Intervenor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intervenor = await _context.Intervenors
                .FirstOrDefaultAsync(m => m.IntervenorId == id);
            if (intervenor == null)
            {
                return NotFound();
            }

            return View(intervenor);
        }

        // GET: Intervenor/Create
        public IActionResult Create()
        {
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "CaseNo");
            ViewData["Company"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Intervenor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IntervenorId,CaseId,CompanyId")] Intervenor intervenor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(intervenor);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: Intervenor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intervenor = await _context.Intervenors.FindAsync(id);
            if (intervenor == null)
            {
                return NotFound();
            }
            return View(intervenor);
        }

        // POST: Intervenor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IntervenorId,CaseId,CompanyId")] Intervenor intervenor)
        {
            if (id != intervenor.IntervenorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(intervenor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntervenorExists(intervenor.IntervenorId))
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
            return View(intervenor);
        }

        // GET: Intervenor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intervenor = await _context.Intervenors
                .FirstOrDefaultAsync(m => m.IntervenorId == id);
            if (intervenor == null)
            {
                return NotFound();
            }

            return View(intervenor);
        }

        // POST: Intervenor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intervenor = await _context.Intervenors.FindAsync(id);
            if (intervenor != null)
            {
                _context.Intervenors.Remove(intervenor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IntervenorExists(int id)
        {
            return _context.Intervenors.Any(e => e.IntervenorId == id);
        }
    }
}
