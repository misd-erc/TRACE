using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                               on task.UserId equals user.Id.ToString() into userJoin
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
        public IActionResult Create()
        {
            ViewData["DocumentId"] = new SelectList(_context.Documents, "DocumentId", "Subject");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: CaseTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                _context.Add(caseTask);
                await _context.SaveChangesAsync();
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
                _context.CaseTasks.Remove(caseTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost, ActionName("CompleteTask")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteTask(long id)
        {
            var caseTask = await _context.CaseTasks.FindAsync(id);
            if (caseTask != null)
            {
                caseTask.ActualCompletionDate = DateOnly.FromDateTime(DateTime.Now);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been updated." });
            }

            return Json(new { success = false, message = "Task not found." });
        }


        private bool CaseTaskExists(long id)
        {
            return _context.CaseTasks.Any(e => e.CaseTaskId == id);
        }
    }
}
