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
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class RelatedCaseController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;
        public RelatedCaseController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
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
            ViewData["ErccaseRelatedId"] = new SelectList(_context.Erccases.Take(20), "ErccaseId", "CaseNo");
            return View();
        }

        // POST: RelatedCase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RelatedCaseId,ErccaseId,ErccaseRelatedId,RelatedBy,DatetimeRelated")] RelatedCase relatedCase)
        {
            if (!ModelState.IsValid)
            {
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                var now = DateTime.Now;

                // Row 1: Original relation
                relatedCase.DatetimeRelated = now;
                relatedCase.RelatedBy = user.Username;

                // Row 2: Reverse relation
                var reverseRelation = new RelatedCase
                {
                    ErccaseId = relatedCase.ErccaseRelatedId,
                    ErccaseRelatedId = relatedCase.ErccaseId,
                    DatetimeRelated = now,
                    RelatedBy = user.Username
                };

                _context.Add(relatedCase);
                _context.Add(reverseRelation);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Success! Data has been saved both ways." });
            }

            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseId);
            ViewData["ErccaseRelatedId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", relatedCase.ErccaseRelatedId);

            return Json(new { success = false, message = "Error! Please check your input." });
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


        [HttpGet]
        public IActionResult SearchCases(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var results = _context.Erccases
                .Where(e => e.CaseNo.Contains(term))
                .OrderBy(e => e.CaseNo)
                .Take(15)
                .Select(e => new
                {
                    id = e.ErccaseId,
                    text = e.CaseNo
                })
                .ToList();

            return Json(results);
        }
    }
}
