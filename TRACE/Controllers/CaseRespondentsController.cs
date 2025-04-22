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
    public class CaseRespondentsController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseRespondentsController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: CaseRespondents
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseRespondents.Include(c => c.Company).Include(c => c.Correspondent).Include(c => c.Erccase);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: CaseRespondents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseRespondent = await _context.CaseRespondents
                .Include(c => c.Company)
                .Include(c => c.Correspondent)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseRespondentId == id);
            if (caseRespondent == null)
            {
                return NotFound();
            }

            return View(caseRespondent);
        }

        // GET: CaseRespondents/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            ViewData["CorrespondentId"] = new SelectList(
                _context.Correspondents
                    .Select(c => new {
                        c.CorrespondentId,
                        FullName = c.Salutation + " " + c.FirstName + " " + c.LastName
                    }),
                "CorrespondentId",
                "FullName"
            );
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: CaseRespondents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseRespondentId,ErccaseId,Remarks,CorrespondentId,CompanyId")] CaseRespondent caseRespondent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseRespondent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseRespondent.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseRespondent.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseRespondent.ErccaseId);
            return View(caseRespondent);
        }

        // GET: CaseRespondents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseRespondent = await _context.CaseRespondents.FindAsync(id);
            if (caseRespondent == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseRespondent.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseRespondent.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseRespondent.ErccaseId);
            return View(caseRespondent);
        }

        // POST: CaseRespondents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseRespondentId,ErccaseId,Remarks,CorrespondentId,CompanyId")] CaseRespondent caseRespondent)
        {
            if (id != caseRespondent.CaseRespondentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseRespondent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseRespondentExists(caseRespondent.CaseRespondentId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseRespondent.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseRespondent.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseRespondent.ErccaseId);
            return View(caseRespondent);
        }

        // GET: CaseRespondents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseRespondent = await _context.CaseRespondents
                .Include(c => c.Company)
                .Include(c => c.Correspondent)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseRespondentId == id);
            if (caseRespondent == null)
            {
                return NotFound();
            }

            return View(caseRespondent);
        }

        // POST: CaseRespondents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseRespondent = await _context.CaseRespondents.FindAsync(id);
            if (caseRespondent != null)
            {
                _context.CaseRespondents.Remove(caseRespondent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseRespondentExists(long id)
        {
            return _context.CaseRespondents.Any(e => e.CaseRespondentId == id);
        }
    }
}
