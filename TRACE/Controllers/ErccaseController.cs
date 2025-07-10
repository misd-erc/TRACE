using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private readonly EventLogger _eventLogger;

        public ErccaseController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper, EventLogger eventLogger)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
            _eventLogger = eventLogger;
        }

        // GET: Erccase
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.Erccases.Include(e => e.CaseCategory).Include(e => e.CaseNature).Include(e => e.CaseStatus);
            return View(await ercdbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCountForEachCategory(string filter)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                string dateFilterCondition = "";
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(filter))
                {
                    DateTime today = DateTime.Today;

                    if (filter == "Today")
                    {
                        dateFilterCondition = "AND CAST(ec.DateFiled AS DATE) = @StartDate";
                        parameters.Add("@StartDate", today);
                    }
                    else if (filter == "Last 7 Days")
                    {
                        dateFilterCondition = "AND ec.DateFiled >= @StartDate";
                        parameters.Add("@StartDate", today.AddDays(-6));
                    }
                    else if (filter == "Last 30 Days")
                    {
                        dateFilterCondition = "AND ec.DateFiled >= @StartDate";
                        parameters.Add("@StartDate", today.AddDays(-29));
                    }
                }

                var sql = $@"
                    SELECT 
                        cc.Category,
                        COUNT(ec.ERCCaseID) AS TotalCases
                    FROM 
                        [ercdb].[cases].[ERCCases] ec
                    JOIN 
                        [ercdb].[cases].[CaseCategories] cc
                        ON ec.CaseCategoryID = cc.CaseCategoryID
                    WHERE 
                        1 = 1
                        {dateFilterCondition}
                    GROUP BY 
                        cc.Category
                    ORDER BY 
                        TotalCases DESC;";

                var result = await connection.QueryAsync<dynamic>(sql, parameters);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateReport(
            int? ERCCaseID,
            int? CaseMilestoneID,
            int? CaseRespondentID,
            int? Year,
            string? Region,
            string? CaseCategoryIDList)
        {
            using var connection = new SqlConnection(_connectionString);

            var results = await connection.QueryAsync(
                "[cases].[sp_GetFilteredERCCases_OR]",
                new
                {
                    ERCCaseID,
                    CaseMilestoneID,
                    CaseRespondentID,
                    Year,
                    Region,
                    CaseCategoryIDList
                },
               commandType: CommandType.StoredProcedure,
                 commandTimeout: 120
            );

            return Ok(results); // returns JSON automatically
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
        public async Task<IActionResult> GetTotalCasesForEachCard()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
                        WITH GroupDefinitions AS (
                            SELECT 'Pending / Ongoing' AS GroupedStatus, 1 AS SortOrder
                            UNION ALL
                            SELECT 'Submitted for Resolution', 2
                            UNION ALL
                            SELECT 'Promulgated / Decided', 3
                            UNION ALL
                            SELECT 'Decided with MR', 4
                            UNION ALL
                            SELECT 'Closed / Dismissed', 5
                        ),
                        GroupedCounts AS (
                            SELECT 
                                CASE 
                                    WHEN cs.Status LIKE '%Pending%' OR cs.Status LIKE '%Ongoing%' THEN 'Pending / Ongoing'
                                    WHEN cs.Status LIKE '%For Resolution%' THEN 'Submitted for Resolution'
                                    WHEN cs.Status LIKE '%Promulgated%' OR cs.Status LIKE '%Decided%' THEN 'Promulgated / Decided'
                                    WHEN cs.Status LIKE '%Decided with MR%' THEN 'Decided with MR'
                                    WHEN cs.Status LIKE '%Closed%' OR cs.Status LIKE '%Dismissed%' THEN 'Closed / Dismissed'
                                END AS GroupedStatus,
                                COUNT(ec.ERCCaseID) AS TotalCases
                            FROM 
                                [ercdb].[cases].[ERCCases] ec
                            JOIN 
                                [ercdb].[cases].[CaseStatuses] cs ON ec.CaseStatusID = cs.CaseStatusID
                            WHERE
                                cs.Status LIKE '%Pending%' OR
                                cs.Status LIKE '%Ongoing%' OR
                                cs.Status LIKE '%For Resolution%' OR
                                cs.Status LIKE '%Promulgated%' OR
                                cs.Status LIKE '%Decided%' OR
                                cs.Status LIKE '%Decided with MR%' OR
                                cs.Status LIKE '%Closed%' OR
                                cs.Status LIKE '%Dismissed%'
                            GROUP BY 
                                CASE 
                                    WHEN cs.Status LIKE '%Pending%' OR cs.Status LIKE '%Ongoing%' THEN 'Pending / Ongoing'
                                    WHEN cs.Status LIKE '%For Resolution%' THEN 'Submitted for Resolution'
                                    WHEN cs.Status LIKE '%Promulgated%' OR cs.Status LIKE '%Decided%' THEN 'Promulgated / Decided'
                                    WHEN cs.Status LIKE '%Decided with MR%' THEN 'Decided with MR'
                                    WHEN cs.Status LIKE '%Closed%' OR cs.Status LIKE '%Dismissed%' THEN 'Closed / Dismissed'
                                END
                        )

                        SELECT 
                            gd.GroupedStatus,
                            ISNULL(gc.TotalCases, 0) AS TotalCases
                        FROM 
                            GroupDefinitions gd
                        LEFT JOIN 
                            GroupedCounts gc ON gd.GroupedStatus = gc.GroupedStatus
                        ORDER BY 
                            gd.SortOrder;


                        ";

                var result = await connection.QueryAsync<dynamic>(sql);
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
        public async Task<IActionResult> GetCaseCount()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = "SELECT COUNT(*) FROM cases.ERCCases";

                var count = await connection.ExecuteScalarAsync<int>(sql);

                return Json(new { TotalCases = count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching case count", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMyCasesDashboard()
        {
            try
            {
                var email = _currentUserHelper.Email;
                var currentUser = email.Split('@')[0];

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"WITH LatestMilestone AS (
                            SELECT 
                                ma.ERCCaseID,
                                ma.CaseMilestoneID,
                                ma.DatetimeAchieved,
                                ROW_NUMBER() OVER (PARTITION BY ma.ERCCaseID ORDER BY ma.DatetimeAchieved DESC) AS rn
                            FROM cases.MilestonesAchieved ma
                        )
                        SELECT 
                            c.ERCCaseID,
                            ISNULL(cm.Milestone, 'N/A') AS MilestoneDescription,
                            lm.DatetimeAchieved,
                            lm.CaseMilestoneID,
                            c.CaseNo,
                            c.Title,
                            ISNULL(cc.Category, 'N/A') AS Category,
                            ISNULL(cn.Nature, 'NOT SET') AS Nature,
                            c.DateFiled,
                            c.DateDocketed,
                            c.DocketedBy,
                            ISNULL(cs.[Status], 'N/A') AS CaseStatus

                        FROM cases.ERCCases c
                        LEFT JOIN cases.CaseRespondents cr ON c.ERCCaseID = cr.ERCCaseID
                        LEFT JOIN cases.CaseCategories cc ON c.CaseCategoryID = cc.CaseCategoryID
                        LEFT JOIN cases.CaseNatures cn ON c.CaseNatureID = cn.CaseNatureID
                        LEFT JOIN cases.CaseStatuses cs ON c.CaseStatusID = cs.CaseStatusID
                        LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID 
                        LEFT JOIN contacts.Correspondents cor ON cr.CorrespondentID = cor.CorrespondentID
                        LEFT JOIN cases.CaseAssignments ca ON c.ERCCaseID = ca.ERCCaseID
                        LEFT JOIN LatestMilestone lm ON c.ERCCaseID = lm.ERCCaseID AND lm.rn = 1
                        LEFT JOIN cases.CaseMilestones cm ON lm.CaseMilestoneID = cm.CaseMilestoneID

                        WHERE ca.UserID = @AssignedTo AND ca.IsActive = 1

                        ORDER BY c.ERCCaseID DESC

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
        public async Task<IActionResult> GetAllMOSCases()
        {
            try
            {
                var email = _currentUserHelper.Email;
                var currentUser = email.Split('@')[0];

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
                        SELECT 
                            c.ERCCaseID,
                            cr.CaseRespondentID,
                            ISNULL(cr.Remarks, 'N/A') AS RespondentRemarks,
                            cr.CorrespondentID AS RespondentCorrespondentID,
                            cr.CompanyID AS RespondentCompanyID,

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
                            ISNULL(ca.UserID, 'N/A') AS AssignedTo

                        FROM cases.ERCCases c
                        LEFT JOIN ercdb.cases.CaseRespondents cr ON c.ERCCaseID = cr.ERCCaseID
                        LEFT JOIN cases.CaseCategories cc ON c.CaseCategoryID = cc.CaseCategoryID
                        LEFT JOIN cases.CaseNatures cn ON c.CaseNatureID = cn.CaseNatureID
                        LEFT JOIN cases.CaseStatuses cs ON c.CaseStatusID = cs.CaseStatusID
                        LEFT JOIN contacts.Companies comp ON cr.CompanyID = comp.CompanyID  
                        LEFT JOIN contacts.Correspondents cor ON cr.CorrespondentID = cor.CorrespondentID
                        LEFT JOIN cases.CaseAssignments ca ON c.ERCCaseID = ca.ERCCaseID

                        WHERE c.DateDocketed IS NOT NULL 
                          AND ca.UserID = @AssignedTo 
                          AND ca.IsActive = 1
                          AND cc.Category IN ('COC', 'RES', 'LRES')

                        ORDER BY c.ERCCaseID DESC;
                    ";

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

        [HttpGet]
        public JsonResult GetCaseNatures(int categoryId)
        {
            var caseNatures = _context.CaseNatures
                .Where(x => x.CaseCategoryId == categoryId)
                .Select(x => new { x.CaseNatureId, x.Nature })
                .ToList();

            return Json(caseNatures);
        }
        [HttpGet]
        public JsonResult GetSubCaseNatures(int natureId)
        {
            var caseNatures = _context.SubCaseNature
                .Where(x => x.CaseNatureId == natureId)
                .Select(x => new { x.CaseNatureId, x.SubNatureName })
                .ToList();

            return Json(caseNatures);
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
        public async Task<IActionResult> Create([Bind("ErccaseId,CaseNo,CaseCategoryId,Title,CaseNatureId,DateFiled,DateDocketed,DocketedBy,CaseStatusId,Synopsis,NoOfFolders,MeterSin,AmountClaimed,AmountSettled,IsArchived,TargetPaissuance,ActualPaissuance,TargetFaissuance,ActualFaissuance,SubmittedForResolution,PrayedForPa,IsApproved,ApprovedBy,DatetimeApproved,CaseBoxLocation,PadeliberationDate,FadeliberationDate,PatargetOrder,FatargetOrder,SubCaseNature")] Erccase erccase)
        {
            try
            {
                var year = DateTime.Now.ToString("yyyy-MM");

                var casecategory = await _context.CaseCategories.FindAsync(erccase.CaseCategoryId);
                if (casecategory == null)
                    return Json(new { success = false, message = $"Case Category with ID {erccase.CaseCategoryId} not found." });

                if (string.IsNullOrWhiteSpace(casecategory.Description))
                    return Json(new { success = false, message = "Case Category description is null or empty." });

                var initalcategory = "-" + GetInitials(casecategory.Description);

                var designatedCaseNo = _context.Erccases
                    .Where(c => c.CaseNo.Contains(initalcategory) && c.CaseNo.Contains(year))
                    .OrderByDescending(c => c.ErccaseId)
                    .FirstOrDefault();

                if (designatedCaseNo == null)
                {
                    erccase.CaseNo = year + "-0001" + initalcategory;
                }
                else
                {
                    var parts = designatedCaseNo.CaseNo.Split('-');
                    if (parts.Length >= 3 && int.TryParse(parts[2], out int numericPart))
                    {
                        numericPart += 1;
                        erccase.CaseNo = $"{year}-{numericPart:D4}" + initalcategory;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to parse the last case number format." });
                    }
                }

                erccase.CaseStatus = await _context.CaseStatuses.FirstOrDefaultAsync(cs => cs.CaseStatusId == 1);

                ModelState.Remove("CaseStatus");
                ModelState.Remove("CaseCategory");
                ModelState.Remove("CaseNo");

                if (ModelState.IsValid)
                {
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    erccase.DocketedBy = user.Username;
                    _context.Add(erccase);
                    await _context.SaveChangesAsync();

                    var fileUploadService = new FileUploadService();
                    int folderCount = erccase.NoOfFolders ?? 1;
                    await fileUploadService.CreateFolders(erccase.CaseNo, folderCount);

                    return Json(new { success = true, message = "Success! Data has been saved." });
                }

                await _eventLogger.LogEventAsync("CREATE", "ERC CASE", "Create Case");

                var errors = ModelState
                    .Where(m => m.Value.Errors.Count > 0)
                    .ToDictionary(
                        m => m.Key,
                        m => m.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                return Json(new { success = false, message = "Validation failed!", errors });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred during case creation.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
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
        public async Task<IActionResult> Edit(long id, [Bind("ErccaseId,CaseNo,CaseCategoryId,Title,CaseNatureId,DateFiled,DateDocketed,DocketedBy,CaseStatusId,Synopsis,NoOfFolders,MeterSin,AmountClaimed,AmountSettled,IsArchived,TargetPaissuance,ActualPaissuance,TargetFaissuance,ActualFaissuance,SubmittedForResolution,PrayedForPa,IsApproved,ApprovedBy,DatetimeApproved,CaseBoxLocation,PadeliberationDate,FadeliberationDate,PatargetOrder,FatargetOrder,SubCaseNature")] Erccase erccase)
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
