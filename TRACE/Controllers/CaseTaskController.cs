using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using TRACE.Context;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class CaseTaskController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;
        public CaseTaskController(ErcdbContext context, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        // GET: CaseTask
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseTasks.Include(c => c.Document).Include(c => c.Erccase);
            return View(await ercdbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseTaskByErcID(int id)
        {
            var tasks = await (from task in _context.CaseTasks
                               join user in _context.Users
                               on task.UserId equals user.Username into userJoin
                               from user in userJoin.DefaultIfEmpty()
                               where task.ErccaseId == id
                               select new
                               {
                                   task.CaseTaskId,
                                   task.Task,
                                   task.TaskedBy,
                                   task.TargetCompletionDate,
                                   task.ActualCompletionDate,
                                   task.UserId,
                                   Username = user != null ? user.Username : "N/A"
                               }).ToListAsync();

            if (tasks == null || !tasks.Any())
            {
                return Json(new { success = false, message = "No tasks found." });
            }

            return Json(new { success = true, data = tasks });
        }
        // GET: CaseTask/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTask = await _context.CaseTasks
                .Include(c => c.Document)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseTaskId == id);
            if (caseTask == null)
            {
                return NotFound();
            }

            return View(caseTask);
        }

        // GET: CaseTask/Create
        public IActionResult Create(int id)
        {
            ViewData["DocumentId"] = new SelectList(_context.Documents, "DocumentId", "Subject");

            //var assignedUsernames = _context.CaseAssignments
            //    .Where(ca => ca.ErccaseId == id && ca.IsActive)
            //    .Select(ca => ca.UserId)
            //    .Distinct()
            //    .ToList();

            var assignedUsers = (from u in _context.Users
                                 join ca in (from caSub in _context.CaseAssignments
                                             where caSub.ErccaseId == id && caSub.IsActive
                                             select caSub.UserId).Distinct()
                                 on u.Username equals ca
                                 select new { u.Username, u.Fullname }).ToList();

            ViewData["UserId"] = new SelectList(assignedUsers, "Username", "Fullname");

            ViewData["ErccaseId"] = id;

            return View();
        }


        // POST: CaseTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CaseTaskId,ErccaseId,UserId,Task,TaskedBy,DatetimeCreated,TargetCompletionDate,ActualCompletionDate,DocumentId")] CaseTask caseTask)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        caseTask.DatetimeCreated = DateTime.Now;
        //        var currentUserName = _currentUserHelper.Email;
        //        var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
        //        caseTask.TaskedBy = user.Username;
        //        _context.Add(caseTask);
        //        await _context.SaveChangesAsync();
        //        return Json(new { success = true, message = "Success! Data has been saved." });
        //    }
        //    ViewData["DocumentId"] = new SelectList(_context.Documents, "DocumentId", "DocumentId", caseTask.DocumentId);
        //    ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseTask.ErccaseId);
        //    return Json(new { success = false, message = "Error! Please check your input." });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseTaskId,ErccaseId,UserId,Task,TaskedBy,DatetimeCreated,TargetCompletionDate,ActualCompletionDate,DocumentId")] CaseTask caseTask)
        {
            if (!ModelState.IsValid)
            {
                caseTask.DatetimeCreated = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                caseTask.TaskedBy = user.Username;
                System.Diagnostics.Debug.WriteLine("TASK CREATED");
                _context.Add(caseTask);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
            
                eventLog.UserId = user.Username;
                eventLog.Event = "CREATE";
                eventLog.Source = "CASE MANAGEMENT";
                eventLog.Category = "Case Task";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();

                var assignedUser = _context.Users.FirstOrDefault(u => u.Username == caseTask.UserId);
                string assignedEmail = assignedUser != null ? $"{assignedUser.Username}@erc.ph" : null;

                var ercCase = _context.Erccases.FirstOrDefault(e => e.ErccaseId == caseTask.ErccaseId);
                System.Diagnostics.Debug.WriteLine("TASK CREATED2");
                try
                {
                    System.Diagnostics.Debug.WriteLine("DEBUG: assignedUser is " + (assignedUser != null ? "NOT null" : "null"));
                    System.Diagnostics.Debug.WriteLine("DEBUG: assignedUser.IsEmailNotif = " + (assignedUser?.IsEmailNotif.ToString() ?? "null"));
                    System.Diagnostics.Debug.WriteLine("DEBUG: ercCase is " + (ercCase != null ? "NOT null" : "null"));
                    System.Diagnostics.Debug.WriteLine("DEBUG: caseTask.TargetCompletionDate.HasValue = " + caseTask.TargetCompletionDate.HasValue);
                    if (assignedUser != null && assignedUser.IsEmailNotif == true && ercCase != null && caseTask.TargetCompletionDate.HasValue)
                    {
                        System.Diagnostics.Debug.WriteLine("TEST HERE: "+assignedEmail);
                        var emailHelper = new EmailNotificationsHelper();
                        emailHelper.SendCaseTaskEmail(
                            assignedUserEmail: assignedEmail,
                            caseNo: ercCase.CaseNo,
                            taskedByUsername: caseTask.TaskedBy,
                            taskDescription: caseTask.Task,
                            targetCompletionDate: caseTask.TargetCompletionDate.Value.ToDateTime(TimeOnly.MinValue)
                        );
                    }
                    if (assignedUser != null && assignedUser.IsSystemNotif == true)
                    {
                        string assignedUserEmail = assignedUser.Email;

                        Notification notif = new Notification();
                        notif.Title = "Case Task Assignment Notification";
                        notif.Message = "You have been assigned a task for CaseNo: '" + ercCase.CaseNo + "' by " + caseTask.TaskedBy;
                        notif.RecipientUserID = assignedUser.Username;
                        notif.CaseID = ercCase.ErccaseId;
                        notif.CreatedAt = DateTime.Now;
                        notif.NotificationType = "user";

                        _context.Notifications.Add(notif);

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                }

                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            ViewData["DocumentId"] = new SelectList(_context.Documents, "DocumentId", "DocumentId", caseTask.DocumentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseTask.ErccaseId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }




        // GET: CaseTask/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTask = await _context.CaseTasks.FindAsync(id);
            if (caseTask == null)
            {
                return NotFound();
            }
            ViewData["DocumentId"] = new SelectList(_context.Documents, "DocumentId", "DocumentId", caseTask.DocumentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseTask.ErccaseId);
            return View(caseTask);
        }

        // POST: CaseTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseTaskId,ErccaseId,UserId,Task,TaskedBy,DatetimeCreated,TargetCompletionDate,ActualCompletionDate,DocumentId")] CaseTask caseTask)
        {
            if (id != caseTask.CaseTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseTask);
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "CASE MANAGEMENT";
                    eventLog.Category = "Case Task";
                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseTaskExists(caseTask.CaseTaskId))
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
            ViewData["DocumentId"] = new SelectList(_context.Documents, "DocumentId", "DocumentId", caseTask.DocumentId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", caseTask.ErccaseId);
            return View(caseTask);
        }

        // GET: CaseTask/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseTask = await _context.CaseTasks
                .Include(c => c.Document)
                .Include(c => c.Erccase)
                .FirstOrDefaultAsync(m => m.CaseTaskId == id);
            if (caseTask == null)
            {
                return NotFound();
            }

            return View(caseTask);
        }

        // POST: CaseTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var caseTask = await _context.CaseTasks.FindAsync(id);
            if (caseTask != null)
            {
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "CASE MANAGEMENT";
                eventLog.Category = "Case Task";
                _context.EventLogs.Add(eventLog);
                _context.CaseTasks.Remove(caseTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(long id)
        {
            var task = await _context.CaseTasks
                .FirstOrDefaultAsync(t => t.CaseTaskId == id);

            if (task == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            if (task.ActualCompletionDate.HasValue)
            {
                return Json(new
                {
                    success = false,
                    message = "Task is already completed on "
                    + task.ActualCompletionDate.Value.ToString("yyyy-MM-dd") + "."
                });
            }
            task.ActualCompletionDate = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error saving completion date." });
            }

            return Json(new
            {
                success = true,
                message = $"Task marked completed on {task.ActualCompletionDate:yyyy-MM-dd}."
            });
        }

        private bool CaseTaskExists(long id)
        {
            return _context.CaseTasks.Any(e => e.CaseTaskId == id);
        }
    }
}
