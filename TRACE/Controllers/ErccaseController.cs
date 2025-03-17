using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class ErccaseController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;

        public ErccaseController(ErcdbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
        }

        // GET: Erccase
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.Erccases.Include(e => e.CaseCategory).Include(e => e.CaseNature).Include(e => e.CaseStatus);
            return View(await ercdbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseRespondents()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"
                    SELECT 
                        cr.CaseRespondentID,
                        cr.ERCCaseID,
                        cr.Remarks AS RespondentRemarks,
                        cr.CorrespondentID AS RespondentCorrespondentID,
                        cr.CompanyID AS RespondentCompanyID,
                
                        c.CaseNo,
                        c.Title,
                        (SELECT Category FROM cases.CaseCategories WHERE CaseCategoryID = c.CaseCategoryID) AS Category,
                        ISNULL((SELECT Nature FROM cases.CaseNatures WHERE CaseNatureID = c.CaseNatureID), 'NOT SET') AS Nature,
                        c.DateFiled,
                        c.DateDocketed,
                        c.DocketedBy,
                        (SELECT [Status] FROM cases.CaseStatuses WHERE CaseStatusID = c.CaseStatusID) AS CaseStatus,
                        comp.CompanyName,  
                        cor.LastName + ' ' + cor.FirstName AS CorrespondentLastName  
                    FROM ercdb.cases.CaseRespondents cr
                    JOIN cases.ERCCases c ON cr.ERCCaseID = c.ERCCaseID
                    LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID  
                    LEFT JOIN ercdb.cases.CaseApplicants ca ON cr.ERCCaseID = ca.ERCCaseID  
                    LEFT JOIN contacts.Correspondents cor ON cr.CorrespondentID = cor.CorrespondentID";

                var result = await connection.QueryAsync<dynamic>(sql);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }


        // GET: Erccase/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var erccase = await _context.Erccases
                .Include(e => e.CaseCategory)
                .Include(e => e.CaseNature)
                .Include(e => e.CaseStatus)
                .FirstOrDefaultAsync(m => m.ErccaseId == id);
            if (erccase == null)
            {
                return NotFound();
            }

            return View(erccase);
        }

        // GET: Erccase/Create
        public IActionResult Create()
        {
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description");
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "Nature");
            ViewData["CaseStatusId"] = new SelectList(_context.CaseStatuses, "CaseStatusId", "Description");
            return View();
        }

        // POST: Erccase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ErccaseId,CaseNo,CaseCategoryId,Title,CaseNatureId,DateFiled,DateDocketed,DocketedBy,CaseStatusId,Synopsis,NoOfFolders,MeterSin,AmountClaimed,AmountSettled,IsArchived,TargetPaissuance,ActualPaissuance,TargetFaissuance,ActualFaissuance,SubmittedForResolution,PrayedForPa,IsApproved,ApprovedBy,DatetimeApproved,CaseBoxLocation,PadeliberationDate,FadeliberationDate,PatargetOrder,FatargetOrder")] Erccase erccase)
        {
            var year = DateTime.Now.ToString("yyyy-MM");

            var designatedCaseNo = _context.Erccases.Where(c => c.CaseNo.Contains("-LC") && c.CaseNo.Contains(year)).OrderByDescending(c => c.ErccaseId).FirstOrDefault(); 
            if(designatedCaseNo == null)
            {
                erccase.CaseNo = year +"-0001-LC";
            }
            else
            {
                string newCaseNumber;
                if (designatedCaseNo != null)
                {
                    // Extract the numeric part and increment
                    var parts = designatedCaseNo.CaseNo.Split('-');
                    int numericPart = int.Parse(parts[2]) + 1;

                    // Format with leading zeros (e.g., 0001, 0002, etc.)
                    erccase.CaseNo = $"{year}-{numericPart:D4}-LC";
                }
                else
                {
                    // If no previous case exists, start from 0001
                    erccase.CaseNo = $"{year}-0001-LC";
                }
            }

           
            erccase.CaseStatus =  _context.CaseStatuses.FirstOrDefault(cs => cs.CaseStatusId == 1); 
            erccase.CaseCategory = _context.CaseCategories.FirstOrDefault(cs => cs.CaseCategoryId == 1);
            ModelState.Remove("CaseStatus");
            ModelState.Remove("CaseCategory");
            ModelState.Remove("CaseNo");

            if (ModelState.IsValid)
            {
                _context.Add(erccase);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description");
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "CaseNature");

            ViewData["CaseStatusId"] = new SelectList(_context.CaseStatuses, "CaseStatusId", "Description");
        var errors = ModelState.Where(m => m.Value.Errors.Count > 0)
                       .ToDictionary(m => m.Key, m => m.Value.Errors.Select(e => e.ErrorMessage).ToList());

return Json(new { success = false, message = "Validation failed!", errors });
        }

        // GET: Erccase/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var erccase = await _context.Erccases.FindAsync(id);
            if (erccase == null)
            {
                return NotFound();
            }
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "CaseCategoryId", erccase.CaseCategoryId);
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "CaseNatureId", erccase.CaseNatureId);
            ViewData["CaseStatusId"] = new SelectList(_context.CaseStatuses, "CaseStatusId", "CaseStatusId", erccase.CaseStatusId);
            return View(erccase);
        }

        // POST: Erccase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ErccaseId,CaseNo,CaseCategoryId,Title,CaseNatureId,DateFiled,DateDocketed,DocketedBy,CaseStatusId,Synopsis,NoOfFolders,MeterSin,AmountClaimed,AmountSettled,IsArchived,TargetPaissuance,ActualPaissuance,TargetFaissuance,ActualFaissuance,SubmittedForResolution,PrayedForPa,IsApproved,ApprovedBy,DatetimeApproved,CaseBoxLocation,PadeliberationDate,FadeliberationDate,PatargetOrder,FatargetOrder")] Erccase erccase)
        {
            if (id != erccase.ErccaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(erccase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ErccaseExists(erccase.ErccaseId))
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
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "CaseCategoryId", erccase.CaseCategoryId);
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "CaseNatureId", erccase.CaseNatureId);
            ViewData["CaseStatusId"] = new SelectList(_context.CaseStatuses, "CaseStatusId", "CaseStatusId", erccase.CaseStatusId);
            return View(erccase);
        }

        // GET: Erccase/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var erccase = await _context.Erccases
                .Include(e => e.CaseCategory)
                .Include(e => e.CaseNature)
                .Include(e => e.CaseStatus)
                .FirstOrDefaultAsync(m => m.ErccaseId == id);
            if (erccase == null)
            {
                return NotFound();
            }

            return View(erccase);
        }

        // POST: Erccase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var erccase = await _context.Erccases.FindAsync(id);
            if (erccase != null)
            {
                _context.Erccases.Remove(erccase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ErccaseExists(long id)
        {
            return _context.Erccases.Any(e => e.ErccaseId == id);
        }
    }
}
