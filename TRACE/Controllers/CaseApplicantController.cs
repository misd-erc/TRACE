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
    public class CaseApplicantController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;

        public CaseApplicantController(ErcdbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
        }

        // GET: CaseApplicant
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseApplicants.Include(c => c.Company).Include(c => c.Correspondent).Include(c => c.Erccase);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseApplicantByErcID(int id)
        { 
          try
            {
                    using var connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync(); // Ensure connection opens

                    var sql = @"SELECT 
                                    ca.CaseApplicantID,
                                    ca.ERCCaseID,
                                    ca.Remarks,
                                    ca.CorrespondentID,
                                    ca.CompanyID,
                                    co.Salutation + ' ' + co.FirstName + ' ' + co.LastName AS FullName
                                FROM [ercdb].[cases].[CaseApplicants] ca
                                INNER JOIN [ercdb].[contacts].Correspondents co
                                    ON ca.CorrespondentID = co.CorrespondentID
                                WHERE ca.ERCCaseID = @id";

                                 var result = await connection.QueryAsync<dynamic>(sql, new { id });
                            return Json(result);
              }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message
            });
          }
        }

        // GET: CaseApplicant/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseApplicant = await _context.CaseApplicants
                .Include(c => c.Company)
                .Include(c => c.Correspondent)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseApplicantId == id);
            if (caseApplicant == null)
            {
                return NotFound();
            }

            return View(caseApplicant);
        }

        // GET: CaseApplicant/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "FirstName");
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: CaseApplicant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseApplicantId,ErccaseId,Remarks,CorrespondentId,CompanyId")] CaseApplicant caseApplicant)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(caseApplicant);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseApplicant.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseApplicant.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseApplicant.ErccaseId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseApplicant/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseApplicant = await _context.CaseApplicants.FindAsync(id);
            if (caseApplicant == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseApplicant.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseApplicant.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseApplicant.ErccaseId);
            return View(caseApplicant);
        }

        // POST: CaseApplicant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseApplicantId,ErccaseId,Remarks,CorrespondentId,CompanyId")] CaseApplicant caseApplicant)
        {
            if (id != caseApplicant.CaseApplicantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseApplicant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseApplicantExists(caseApplicant.CaseApplicantId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", caseApplicant.CompanyId);
            ViewData["CorrespondentId"] = new SelectList(_context.Correspondents, "CorrespondentId", "CorrespondentId", caseApplicant.CorrespondentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseApplicant.ErccaseId);
            return View(caseApplicant);
        }

        // GET: CaseApplicant/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseApplicant = await _context.CaseApplicants
                .Include(c => c.Company)
                .Include(c => c.Correspondent)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseApplicantId == id);
            if (caseApplicant == null)
            {
                return NotFound();
            }

            return View(caseApplicant);
        }

        // POST: CaseApplicant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseApplicant = await _context.CaseApplicants.FindAsync(id);
            if (caseApplicant != null)
            {
                _context.CaseApplicants.Remove(caseApplicant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseApplicantExists(long id)
        {
            return _context.CaseApplicants.Any(e => e.CaseApplicantId == id);
        }
    }
}
