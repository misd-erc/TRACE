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
    public class CaseAssignmentController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseAssignmentController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: CaseAssignment
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseAssignments.Include(c => c.Erccase).Include(c => c.HandlingOfficerType);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: CaseAssignment/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAssignment = await _context.CaseAssignments
                .Include(c => c.Erccase)
                .Include(c => c.HandlingOfficerType)
                .FirstOrDefaultAsync(m => m.CaseAssignmentId == id);
            if (caseAssignment == null)
            {
                return NotFound();
            }

            return View(caseAssignment);
        }

        // GET: CaseAssignment/Create
        public IActionResult Create()
        {
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            ViewData["HandlingOfficerTypeId"] = new SelectList(_context.HandlingOfficerTypes, "HandlingOfficerTypeId", "HandlingOfficerTypeId");
            return View();
        }

        // POST: CaseAssignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseAssignmentId,UserId,ErccaseId,DateAssigned,AssignedBy,HandlingOfficerTypeId")] CaseAssignment caseAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseAssignment.ErccaseId);
            ViewData["HandlingOfficerTypeId"] = new SelectList(_context.HandlingOfficerTypes, "HandlingOfficerTypeId", "HandlingOfficerTypeId", caseAssignment.HandlingOfficerTypeId);
            return View(caseAssignment);
        }

        // GET: CaseAssignment/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAssignment = await _context.CaseAssignments.FindAsync(id);
            if (caseAssignment == null)
            {
                return NotFound();
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseAssignment.ErccaseId);
            ViewData["HandlingOfficerTypeId"] = new SelectList(_context.HandlingOfficerTypes, "HandlingOfficerTypeId", "HandlingOfficerTypeId", caseAssignment.HandlingOfficerTypeId);
            return View(caseAssignment);
        }

        // POST: CaseAssignment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseAssignmentId,UserId,ErccaseId,DateAssigned,AssignedBy,HandlingOfficerTypeId")] CaseAssignment caseAssignment)
        {
            if (id != caseAssignment.CaseAssignmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseAssignmentExists(caseAssignment.CaseAssignmentId))
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
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseAssignment.ErccaseId);
            ViewData["HandlingOfficerTypeId"] = new SelectList(_context.HandlingOfficerTypes, "HandlingOfficerTypeId", "HandlingOfficerTypeId", caseAssignment.HandlingOfficerTypeId);
            return View(caseAssignment);
        }

        // GET: CaseAssignment/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseAssignment = await _context.CaseAssignments
                .Include(c => c.Erccase)
                .Include(c => c.HandlingOfficerType)
                .FirstOrDefaultAsync(m => m.CaseAssignmentId == id);
            if (caseAssignment == null)
            {
                return NotFound();
            }

            return View(caseAssignment);
        }

        // POST: CaseAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseAssignment = await _context.CaseAssignments.FindAsync(id);
            if (caseAssignment != null)
            {
                _context.CaseAssignments.Remove(caseAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseAssignmentExists(long id)
        {
            return _context.CaseAssignments.Any(e => e.CaseAssignmentId == id);
        }
    }
}
