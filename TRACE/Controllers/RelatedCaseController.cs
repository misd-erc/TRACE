using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class RelatedCaseController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        public RelatedCaseController(ErcdbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
        }

        // GET: RelatedCase
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.RelatedCases.Include(r => r.Erccase).Include(r => r.ErccaseRelated);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseRelatedByErcID(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"SELECT 
                                rc.RelatedCaseID,
                                rc.ERCCaseID,
                                e1.CaseNo AS ERCCaseNo,
                                e1.Title AS ERCCaseTitle,
                                rc.ERCCaseRelatedID,
                                e2.CaseNo AS RelatedCaseNo,
                                e2.Title AS RelatedCaseTitle,
                                rc.RelatedBy,
                                rc.DatetimeRelated
                            FROM 
                                [ercdb].[cases].[RelatedCases] rc
                            LEFT JOIN 
                                [ercdb].[cases].[ERCCases] e1 ON rc.ERCCaseID = e1.ERCCaseID
                            LEFT JOIN 
                                [ercdb].[cases].[ERCCases] e2 ON rc.ERCCaseRelatedID = e2.ERCCaseID
                            WHERE 
                                rc.ERCCaseID = @id";

                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }

        // GET: RelatedCase/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedCase = await _context.RelatedCases
                .Include(r => r.Erccase)
                .Include(r => r.ErccaseRelated)
                .FirstOrDefaultAsync(m => m.RelatedCaseId == id);
            if (relatedCase == null)
            {
                return NotFound();
            }

            return View(relatedCase);
        }

        // GET: RelatedCase/Create
        public IActionResult Create()
        {
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            ViewData["ErccaseRelatedId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: RelatedCase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RelatedCaseId,ErccaseId,ErccaseRelatedId,RelatedBy,DatetimeRelated")] RelatedCase relatedCase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relatedCase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseId);
            ViewData["ErccaseRelatedId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseRelatedId);
            return View(relatedCase);
        }

        // GET: RelatedCase/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedCase = await _context.RelatedCases.FindAsync(id);
            if (relatedCase == null)
            {
                return NotFound();
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseId);
            ViewData["ErccaseRelatedId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseRelatedId);
            return View(relatedCase);
        }

        // POST: RelatedCase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("RelatedCaseId,ErccaseId,ErccaseRelatedId,RelatedBy,DatetimeRelated")] RelatedCase relatedCase)
        {
            if (id != relatedCase.RelatedCaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relatedCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelatedCaseExists(relatedCase.RelatedCaseId))
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
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseId);
            ViewData["ErccaseRelatedId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseRelatedId);
            return View(relatedCase);
        }

        // GET: RelatedCase/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedCase = await _context.RelatedCases
                .Include(r => r.Erccase)
                .Include(r => r.ErccaseRelated)
                .FirstOrDefaultAsync(m => m.RelatedCaseId == id);
            if (relatedCase == null)
            {
                return NotFound();
            }

            return View(relatedCase);
        }

        // POST: RelatedCase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var relatedCase = await _context.RelatedCases.FindAsync(id);
            if (relatedCase != null)
            {
                _context.RelatedCases.Remove(relatedCase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RelatedCaseExists(long id)
        {
            return _context.RelatedCases.Any(e => e.RelatedCaseId == id);
        }
    }
}
