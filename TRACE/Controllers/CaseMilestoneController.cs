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
    public class CaseMilestoneController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseMilestoneController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
        }

        // GET: CaseMilestone
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseMilestones.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> CheckMilestoneIsAchieved(int erccaseId, int casemilestoneId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"
            SELECT [MilestoneAchievedID],
                   [ERCCaseID],
                   [CaseMilestoneID],
                   [DatetimeAchieved],
                   [PercentAchieved]
            FROM [ercdb].[cases].[MilestonesAchieved]
            WHERE ERCCaseID = @erccaseId AND CaseMilestoneID = @casemilestoneId";

                // Pass both parameters correctly
                var result = await connection.QueryAsync<dynamic>(sql, new { erccaseId, casemilestoneId });

                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetMilestoneOfCases(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"
                    SELECT 
                        cmtm.CaseMilestoneTemplateID,
                        cmtm.CaseMilestoneID,
                        cm.Milestone,
                        cm.Description
                    FROM 
                        [ercdb].[cases].[CaseMilestoneTemplateMembers] cmtm
                    JOIN 
                        [ercdb].[cases].[CaseMilestones] cm 
                        ON cmtm.CaseMilestoneID = cm.CaseMilestoneID
                    INNER JOIN 
                        [ercdb].[cases].[CaseMilestoneTemplates] cmt 
                        ON cmtm.CaseMilestoneTemplateID = cmt.CaseMilestoneTemplateID
                    WHERE 
                        cmt.CaseCategoryID = @id";

                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseMilestones()
        {
            var categories = await _context.CaseMilestones.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: CaseMilestone/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestone = await _context.CaseMilestones
                .FirstOrDefaultAsync(m => m.CaseMilestoneId == id);
            if (caseMilestone == null)
            {
                return NotFound();
            }

            return View(caseMilestone);
        }

        // GET: CaseMilestone/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseMilestone/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseMilestoneId,Milestone,Description")] CaseMilestone caseMilestone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseMilestone);
                await _context.SaveChangesAsync();
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "CREATE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "Case Milestone";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseMilestone/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestone = await _context.CaseMilestones.FindAsync(id);
            if (caseMilestone == null)
            {
                return NotFound();
            }
            return View(caseMilestone);
        }

        // POST: CaseMilestone/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseMilestoneId,Milestone,Description")] CaseMilestone caseMilestone)
        {
            if (id != caseMilestone.CaseMilestoneId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseMilestone);
                    await _context.SaveChangesAsync();
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "CONTENT MANAGEMENT";
                    eventLog.Category = "Case Milestone";
                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseMilestoneExists(caseMilestone.CaseMilestoneId))
                    {
                        return Json(new { success = false, message = "Error! Data not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true, message = "Success! Data has been updated." });
            }
            return Json(new { success = true, message = "Success! Data has been updated." });
        }

        // GET: CaseMilestone/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestone = await _context.CaseMilestones
                .FirstOrDefaultAsync(m => m.CaseMilestoneId == id);
            if (caseMilestone == null)
            {
                return NotFound();
            }

            return View(caseMilestone);
        }

        // POST: CaseMilestone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseMilestone = await _context.CaseMilestones.FindAsync(id);
            if (caseMilestone != null)
            {
                _context.CaseMilestones.Remove(caseMilestone);
            }
            EventLog eventLog = new EventLog();
            eventLog.EventDatetime = DateTime.Now;
            var currentUserName = _currentUserHelper.Email;
            var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
            eventLog.UserId = user.Username;
            eventLog.Event = "DELETE";
            eventLog.Source = "CONTENT MANAGEMENT";
            eventLog.Category = "Case Milestone";
            _context.EventLogs.Add(eventLog);
            await _context.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));
        }

        private bool CaseMilestoneExists(long id)
        {
            return _context.CaseMilestones.Any(e => e.CaseMilestoneId == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetFolderMilestoneName(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Milestone FROM ercdb.cases.CaseMilestones WHERE CaseMilestoneID = @Id";
                var milestoneName = await connection.QueryFirstOrDefaultAsync<string>(sql, new { Id = id });

                if (milestoneName != null)
                    return Ok(new { success = true, milestone = milestoneName });
                else
                    return NotFound(new { success = false, message = "Milestone not found." });
            }
        }
    }
}
