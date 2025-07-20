using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseDetailsController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;

        public CaseDetailsController(ErcdbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
        }

        // GET: CaseDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseDetails.ToListAsync());
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCaseDetails(string id)
        {
            
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"
                    SELECT 
                            a.*,
                            (SELECT [Description] FROM cases.CaseCategories WHERE CaseCategoryID = a.CaseCategoryID) AS CaseCategory,
                            (SELECT [Description] FROM cases.CaseNatures WHERE CaseNatureID = a.CaseNatureID) AS CaseNature,
                            (SELECT [Status] FROM cases.CaseStatuses WHERE CaseStatusID = a.CaseStatusID) AS CaseStatus,
                            (CASE WHEN a.PrayedForPA = 1 THEN 'Yes' ELSE 'No' END) AS hasPrayedForPA,

                            comp.CompanyName,

                            cor.LastName + ' ' + cor.FirstName AS RespondentFullName,

                            acor.FirstName + ' ' + acor.LastName AS ApplicantFullName

                        FROM cases.ERCCases a

                        LEFT JOIN ercdb.cases.CaseRespondents cr ON cr.ERCCaseID = a.ERCCaseID
                        LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID
                        LEFT JOIN contacts.Correspondents cor ON cr.CorrespondentID = cor.CorrespondentID

                        LEFT JOIN ercdb.cases.CaseApplicants ca ON ca.ERCCaseID = a.ERCCaseID
                        LEFT JOIN contacts.Correspondents acor ON ca.CorrespondentID = acor.CorrespondentID

                        WHERE a.CaseNo = @id;";

                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }
        // GET: CaseDetails/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseDetails = await _context.CaseDetails
                .FirstOrDefaultAsync(m => m.ERCCaseID == id);
            if (caseDetails == null)
            {
                return NotFound();
            }

            return View(caseDetails);
        }

        // GET: CaseDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ERCCaseID,CaseTitle,CaseNumber,CaseCategory,CaseNature,CaseStatus,HasPrayedForPA,DateFiled,Complainant,Respondent")] CaseDetails caseDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseDetails);
        }

        // GET: CaseDetails/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseDetails = await _context.CaseDetails.FindAsync(id);
            if (caseDetails == null)
            {
                return NotFound();
            }
            return View(caseDetails);
        }

        // POST: CaseDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ERCCaseID,CaseTitle,CaseNumber,CaseCategory,CaseNature,CaseStatus,HasPrayedForPA,DateFiled,Complainant,Respondent")] CaseDetails caseDetails)
        {
            if (id != caseDetails.ERCCaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseDetailsExists(caseDetails.ERCCaseID))
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
            return View(caseDetails);
        }

        // GET: CaseDetails/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseDetails = await _context.CaseDetails
                .FirstOrDefaultAsync(m => m.ERCCaseID == id);
            if (caseDetails == null)
            {
                return NotFound();
            }

            return View(caseDetails);
        }

        // POST: CaseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseDetails = await _context.CaseDetails.FindAsync(id);
            if (caseDetails != null)
            {
                _context.CaseDetails.Remove(caseDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseDetailsExists(long id)
        {
            return _context.CaseDetails.Any(e => e.ERCCaseID == id);
        }
    }
}
