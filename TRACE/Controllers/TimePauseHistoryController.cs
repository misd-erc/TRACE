using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class TimePauseHistoryController : Controller
    {
        private readonly ErcdbContext _context;

        public TimePauseHistoryController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: TimePauseHistory
        public async Task<IActionResult> Index()
        {
            return View(await _context.TimePauseHistories.ToListAsync());
        }

        // GET: TimePauseHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timePauseHistory = await _context.TimePauseHistories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timePauseHistory == null)
            {
                return NotFound();
            }

            return View(timePauseHistory);
        }

        // GET: TimePauseHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimePauseHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Savestatus([Bind("DateUpdated,Status,ErcId,UserId,Remarks")] TimePauseHistory timePauseHistory)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(string.Join("; ", errors));
            }

            _context.Add(timePauseHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TimePauseHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timePauseHistory = await _context.TimePauseHistories.FindAsync(id);
            if (timePauseHistory == null)
            {
                return NotFound();
            }
            return View(timePauseHistory);
        }

        // POST: TimePauseHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateUpdated,Status,ErcId,UserId,Remarks")] TimePauseHistory timePauseHistory)
        {
            if (id != timePauseHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timePauseHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimePauseHistoryExists(timePauseHistory.Id))
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
            return View(timePauseHistory);
        }

        // GET: TimePauseHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timePauseHistory = await _context.TimePauseHistories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timePauseHistory == null)
            {
                return NotFound();
            }

            return View(timePauseHistory);
        }

        // POST: TimePauseHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timePauseHistory = await _context.TimePauseHistories.FindAsync(id);
            if (timePauseHistory != null)
            {
                _context.TimePauseHistories.Remove(timePauseHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimePauseHistoryExists(int id)
        {
            return _context.TimePauseHistories.Any(e => e.Id == id);
        }


        [HttpGet]
        public async Task<IActionResult> ByCaseId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var isJsonRequest = Request.Headers.ContentType.Contains("application/json")
                    || Request.Headers.XRequestedWith == "XMLHttpRequest";
            if (!isJsonRequest)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }

            var timePauseHistory = await _context.TimePauseHistories.OrderByDescending(t => t.Id)
                .FirstOrDefaultAsync(t => t.ErcId == id);
            if (timePauseHistory == null)
            {
                return NotFound();
            }

            return Json(timePauseHistory);
        }
    }
}
