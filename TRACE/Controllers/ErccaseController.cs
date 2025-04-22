using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TRACE.BlobStorage;
using TRACE.Context;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class ErccaseController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public ErccaseController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
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
	                cr.ERCCaseID,

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
        
        [HttpGet]
        public async Task<IActionResult> GetAllLetterComplaints()
        {
            try
            {
                var email = _currentUserHelper.Email;
                var currentUser = email.Split('@')[0];

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"SELECT 
                                    c.ERCCaseID,
                                    c.CaseNo,
                                    c.Title,
                                    ISNULL(cc.Category, 'N/A') AS Category,
                                    ISNULL(cn.Nature, 'NOT SET') AS Nature,
                                    c.DateFiled,
                                    c.DateDocketed,
                                    c.DocketedBy,
                                    ISNULL(cs.[Status], 'N/A') AS CaseStatus,

                                    -- CONCATENATE ALL COMPANIES per case
                                    ISNULL(STRING_AGG(comp.CompanyName, ', '), 'N/A') AS CompanyName

                                FROM cases.ERCCases c
                                LEFT JOIN ercdb.cases.CaseRespondents cr ON c.ERCCaseID = cr.ERCCaseID
                                LEFT JOIN cases.CaseCategories cc ON c.CaseCategoryID = cc.CaseCategoryID
                                LEFT JOIN cases.CaseNatures cn ON c.CaseNatureID = cn.CaseNatureID
                                LEFT JOIN cases.CaseStatuses cs ON c.CaseStatusID = cs.CaseStatusID
                                LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID  
                                LEFT JOIN cases.CaseAssignments ca ON c.ERCCaseID = ca.ERCCaseID

                                WHERE ca.UserID = @AssignedTo AND ca.IsActive = 1

                                GROUP BY 
                                    c.ERCCaseID, c.CaseNo, c.Title, cc.Category, cn.Nature, 
                                    c.DateFiled, c.DateDocketed, c.DocketedBy, cs.Status

                                ORDER BY c.ERCCaseID DESC;

                            "
                    ;

                var result = await connection.QueryAsync<dynamic>(sql, new { AssignedTo = currentUser });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCases()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"SELECT 
                                c.ERCCaseID,
                                ISNULL(cr.CaseRespondentID, NULL) AS CaseRespondentID,
                                ISNULL(cr.Remarks, 'N/A') AS RespondentRemarks,
                                ISNULL(cr.CorrespondentID, NULL) AS RespondentCorrespondentID,
                                ISNULL(cr.CompanyID, NULL) AS RespondentCompanyID,

                                c.CaseNo,
                                c.Title,
                                ISNULL(cc.Category, 'N/A') AS Category,
                                ISNULL(cn.Nature, 'NOT SET') AS Nature,
                                c.DateFiled,
                                c.DateDocketed,
                                c.DocketedBy,
                                ISNULL(cs.[Status], 'N/A') AS CaseStatus,

                                ISNULL(comp.CompanyName, 'N/A') AS CompanyName,  
                                ISNULL(cor.LastName + ' ' + cor.FirstName, 'N/A') AS CorrespondentLastName  

                            FROM cases.ERCCases c
                            LEFT JOIN ercdb.cases.CaseRespondents cr ON c.ERCCaseID = cr.ERCCaseID
                            LEFT JOIN cases.CaseCategories cc ON c.CaseCategoryID = cc.CaseCategoryID
                            LEFT JOIN cases.CaseNatures cn ON c.CaseNatureID = cn.CaseNatureID
                            LEFT JOIN cases.CaseStatuses cs ON c.CaseStatusID = cs.CaseStatusID
                            LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID  
                            LEFT JOIN contacts.Correspondents cor ON cr.CorrespondentID = cor.CorrespondentID order by c.ERCCaseID desc

                            
                            "
                    ;

                var result = await connection.QueryAsync<dynamic>(sql);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocketedCases()
        {
            try
            {
                var email = _currentUserHelper.Email;
                var currentUser = email.Split('@')[0];

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"SELECT 
                            c.ERCCaseID,
                            ISNULL(cr.CaseRespondentID, NULL) AS CaseRespondentID,
                            ISNULL(cr.Remarks, 'N/A') AS RespondentRemarks,
                            ISNULL(cr.CorrespondentID, NULL) AS RespondentCorrespondentID,
                            ISNULL(cr.CompanyID, NULL) AS RespondentCompanyID,

                            c.CaseNo,
                            c.Title,
                            ISNULL(cc.Category, 'N/A') AS Category,
                            ISNULL(cn.Nature, 'NOT SET') AS Nature,
                            c.DateFiled,
                            c.DateDocketed,
                            c.DocketedBy,
                            ISNULL(cs.[Status], 'N/A') AS CaseStatus,

                            ISNULL(comp.CompanyName, 'N/A') AS CompanyName,  
                            ISNULL(cor.LastName + ' ' + cor.FirstName, 'N/A') AS CorrespondentLastName,
	                        ISNULL (ca.UserID, 'N/A') AS AssignedTo

                        FROM cases.ERCCases c
                        LEFT JOIN ercdb.cases.CaseRespondents cr ON c.ERCCaseID = cr.ERCCaseID
                        LEFT JOIN cases.CaseCategories cc ON c.CaseCategoryID = cc.CaseCategoryID
                        LEFT JOIN cases.CaseNatures cn ON c.CaseNatureID = cn.CaseNatureID
                        LEFT JOIN cases.CaseStatuses cs ON c.CaseStatusID = cs.CaseStatusID
                        LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID  
                        LEFT JOIN contacts.Correspondents cor ON cr.CorrespondentID = cor.CorrespondentID
                        LEFT JOIN cases.CaseAssignments ca ON c.ERCCaseID = ca.ERCCaseID

                        WHERE c.DateDocketed IS NOT NULL 
                        AND ca.UserID = @AssignedTo AND ca.IsActive = 1
                        ORDER BY c.ERCCaseID DESC"

                    ;

                var result = await connection.QueryAsync<dynamic>(sql, new { AssignedTo = currentUser });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetLastCases(long casecategoryId)
        {
            var result = "";
            var year = DateTime.Now.ToString("yyyy-MM");
            var casecategory = _context.CaseCategories.Find(casecategoryId);
            if (casecategory == null)
            {
                return BadRequest("Invalid case category ID.");
            }

            var initalcategory = "-" + GetInitials(casecategory.Description);
            var designatedCaseNo = _context.Erccases.Where(c => c.CaseNo.Contains(initalcategory) && c.CaseNo.Contains(year)).OrderByDescending(c => c.ErccaseId).FirstOrDefault();
            if (designatedCaseNo == null)
            {
                result = year + "-0001" + initalcategory;
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
                    result = $"{year}-{numericPart:D4}-" + initalcategory;
                }
                else
                {
                    // If no previous case exists, start from 0001
                    result = $"{year}-0001-" + initalcategory;
                }
            }

            return Json(result);
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

        static string GetInitials(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string[] words = input.Split(' ');
            string initials = "";

            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                    initials += char.ToUpper(word[0]);
            }

            return initials;
        }

        // POST: Erccase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ErccaseId,CaseNo,CaseCategoryId,Title,CaseNatureId,DateFiled,DateDocketed,DocketedBy,CaseStatusId,Synopsis,NoOfFolders,MeterSin,AmountClaimed,AmountSettled,IsArchived,TargetPaissuance,ActualPaissuance,TargetFaissuance,ActualFaissuance,SubmittedForResolution,PrayedForPa,IsApproved,ApprovedBy,DatetimeApproved,CaseBoxLocation,PadeliberationDate,FadeliberationDate,PatargetOrder,FatargetOrder")] Erccase erccase)
        {
            
            var year = DateTime.Now.ToString("yyyy-MM");
            var casecategory = _context.CaseCategories.Find(erccase.CaseCategoryId);

            var initalcategory = "-"+GetInitials(casecategory.Description);
            var designatedCaseNo = _context.Erccases.Where(c => c.CaseNo.Contains(initalcategory) && c.CaseNo.Contains(year)).OrderByDescending(c => c.ErccaseId).FirstOrDefault(); 
            if(designatedCaseNo == null)
            {
                erccase.CaseNo = year +"-0001"+initalcategory;
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
                    erccase.CaseNo = $"{year}-{numericPart:D4}-"+ initalcategory;
                }
                else
                {
                    // If no previous case exists, start from 0001
                    erccase.CaseNo = $"{year}-0001-" + initalcategory;
                }
            }

           
            erccase.CaseStatus =  _context.CaseStatuses.FirstOrDefault(cs => cs.CaseStatusId == 1); 
            erccase.CaseCategory = _context.CaseCategories.FirstOrDefault(cs => cs.CaseCategoryId == 1);
            ModelState.Remove("CaseStatus");
            ModelState.Remove("CaseCategory");
            ModelState.Remove("CaseNo");

            if (ModelState.IsValid)
            {
                FileUploadService fileUploadService = new FileUploadService();
                int folderCount = erccase.NoOfFolders ?? 1;
               await fileUploadService.CreateFolders(erccase.CaseNo, folderCount);
                _context.Add(erccase);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            EventLog eventLog = new EventLog();
            eventLog.EventDatetime = DateTime.Now;
            var currentUserName = _currentUserHelper.Email;
            var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
            eventLog.UserId = user.Username;
            eventLog.Event = "CREATE";
            eventLog.Source = "ERC CASE";
            eventLog.Category = "Create Case";
            _context.EventLogs.Add(eventLog);
            await _context.SaveChangesAsync();

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
            ViewData["CaseCategoryId"] = new SelectList(_context.CaseCategories, "CaseCategoryId", "Description", erccase.CaseCategoryId);
            ViewData["CaseNatureId"] = new SelectList(_context.CaseNatures, "CaseNatureId", "Nature", erccase.CaseNatureId);
            ViewData["CaseStatusId"] = new SelectList(_context.CaseStatuses, "CaseStatusId", "Description", erccase.CaseStatusId);
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
                return Json(new { success = false, message = "Case not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(erccase);
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "ERC CASE";
                    eventLog.Category = "Edit Case";
                    _context.EventLogs.Add(eventLog);

                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Case updated successfully!" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ErccaseExists(erccase.ErccaseId))
                    {
                        return Json(new { success = false, message = "Case no longer exists." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "An error occurred while updating." });
                    }
                }
            }

            return Json(new { success = false, message = "Validation failed." });
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
