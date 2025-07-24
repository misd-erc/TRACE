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
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseAssignmentController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;
        private readonly EventLogger _eventLogger;

        public CaseAssignmentController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper, EventLogger eventLogger = null)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
            _eventLogger = eventLogger;
        }

        // GET: CaseAssignment
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseAssignments.Include(c => c.Erccase).Include(c => c.HandlingOfficerType);
            return View(await ercdbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseAssignmentByErcID(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"SELECT 
                                ca.CaseAssignmentID,
                                ca.UserID,
                                ca.ERCCaseID,
                                ca.DateAssigned,
                                ca.AssignedBy,
                                ca.HandlingOfficerTypeID,
                                ca.IsActive,
                                ho.OfficerType,
                                ho.Description AS OfficerDescription
                            FROM 
                                [icdms2025].[cases].[CaseAssignments] ca
                            LEFT JOIN 
                                [icdms2025].[cases].[HandlingOfficerTypes] ho ON ca.HandlingOfficerTypeID = ho.HandlingOfficerTypeID
                            WHERE 
                                ca.ERCCaseID = @id AND ca.IsActive=1";

                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseAssignmentHistoryByErcID(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"SELECT 
                                ca.CaseAssignmentID,
                                ca.UserID,
                                ca.ERCCaseID,
                                ca.DateAssigned,
                                ca.AssignedBy,
                                ca.HandlingOfficerTypeID,
                                ca.IsActive,
                                ca.DateRemoved,
                                ho.OfficerType,
                                ho.Description AS OfficerDescription
                            FROM 
                                [icdms2025].[cases].[CaseAssignments] ca
                            LEFT JOIN 
                                [icdms2025].[cases].[HandlingOfficerTypes] ho ON ca.HandlingOfficerTypeID = ho.HandlingOfficerTypeID
                            WHERE 
                                ca.ERCCaseID = @id AND ca.IsActive=0";

                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
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
            ViewData["Users"] = new SelectList(_context.Users, "Username", "Fullname");
            ViewData["HandlingOfficerTypeId"] = new SelectList(_context.HandlingOfficerTypes, "HandlingOfficerTypeId", "OfficerType");
            return View();
        }

        // POST: CaseAssignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseAssignmentId,UserId,ErccaseId,DateAssigned,AssignedBy,HandlingOfficerTypeId")] CaseAssignment caseAssignment)
        {
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseAssignment.ErccaseId);
            ViewData["HandlingOfficerTypeId"] = new SelectList(_context.HandlingOfficerTypes, "HandlingOfficerTypeId", "HandlingOfficerTypeId", caseAssignment.HandlingOfficerTypeId);

            if (!ModelState.IsValid)
            {
                caseAssignment.IsActive = true;

                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                caseAssignment.AssignedBy = user.Username;

                caseAssignment.DateAssigned = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(caseAssignment);
                await _eventLogger.LogEventAsync("CREATE", "CASE MANAGEMENT", "CaseAssignment");
                await _context.SaveChangesAsync();

                var assignedUsername = caseAssignment.UserId;
                var assignedByUsername = caseAssignment.AssignedBy;
                var erccaseId = caseAssignment.ErccaseId;

                var ercCase = _context.Erccases.FirstOrDefault(c => c.ErccaseId == erccaseId);

                if (!string.IsNullOrEmpty(assignedUsername) && ercCase != null)
                {
                    var assignedUser = _context.Users.FirstOrDefault(u => u.Username == assignedUsername);

                    if (assignedUser != null && assignedUser.IsEmailNotif == true)
                    {
                        string assignedUserEmail = assignedUser.Email;

                        var emailHelper = new EmailNotificationsHelper();
                        emailHelper.SendCaseAssignmentEmail(assignedUserEmail, ercCase.CaseNo, assignedByUsername);
                    }
                    if (assignedUser != null && assignedUser.IsSystemNotif == true)
                    {
                        string assignedUserEmail = assignedUser.Email;

                        Notification notif = new Notification();
                        notif.Title = "Case Assignment Notification";
                        notif.Message = "You have been assigned to a new case CaseNo: '"+ ercCase.CaseNo + "' by "+ assignedByUsername;
                        notif.RecipientUserID = assignedByUsername;
                        notif.CaseID = ercCase.ErccaseId;
                        notif.CreatedAt = DateTime.Now;
                        notif.NotificationType = "user";

                        _context.Notifications.Add(notif);
                             
                        await _context.SaveChangesAsync();
                    }
                }

                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }




        [HttpPost]
        public async Task<IActionResult> ArchiveUserAssign(long id)
        {
            try
            {
                // Debug log to check the value of 'id'
                Console.WriteLine($"Received ID: {id}");

                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
                    UPDATE [icdms2025].[cases].[CaseAssignments] 
                    SET IsActive = 0, 
                        DateRemoved = GETDATE() 
                    WHERE CaseAssignmentID = @id";

                // Debug log the SQL query with the ID
                Console.WriteLine($"Executing SQL: {sql} with ID = {id}");

                var rowsAffected = await connection.ExecuteAsync(sql, new { id });

                if (rowsAffected > 0)
                {
                    return Json(new { success = true, message = "User has been removed from this case!" });
                }
                else
                {
                    return Json(new { success = false, message = $"No record updated. Check if the ID is correct. Attempted ID = {id}" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while archiving the user assignment.", error = ex.Message });
            }
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
                    await _eventLogger.LogEventAsync("EDIT", "CASE MANAGEMENT", "CaseAssignment");
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
                await _eventLogger.LogEventAsync("DELETE", "CASE MANAGEMENT", "CaseAssignment");
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Success! Data has been deleted." });
        }

        private bool CaseAssignmentExists(long id)
        {
            return _context.CaseAssignments.Any(e => e.CaseAssignmentId == id);
        }
    }
}
