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
    public class CaseAssignmentController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseAssignmentController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
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
                                ho.OfficerType,
                                ho.Description AS OfficerDescription
                            FROM 
                                [ercdb].[cases].[CaseAssignments] ca
                            LEFT JOIN 
                                [ercdb].[cases].[HandlingOfficerTypes] ho ON ca.HandlingOfficerTypeID = ho.HandlingOfficerTypeID
                            WHERE 
                                ca.ERCCaseID = @id";

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
            ViewData["Users"] = new SelectList(_context.Users, "Username", "Username");
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
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                caseAssignment.AssignedBy = user.Username;

                caseAssignment.DateAssigned = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(caseAssignment);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
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
