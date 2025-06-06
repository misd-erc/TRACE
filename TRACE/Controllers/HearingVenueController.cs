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
    public class HearingVenueController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public HearingVenueController(ErcdbContext context, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }
        [HttpGet]
        public async Task<IActionResult> GetHearingVenues()
        {
            var categories = await _context.HearingVenues.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }
        // GET: HearingVenue
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.HearingVenues.Include(h => h.City);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: HearingVenue/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingVenue = await _context.HearingVenues
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.HearingVenueId == id);
            if (hearingVenue == null)
            {
                return NotFound();
            }

            return View(hearingVenue);
        }

        // GET: HearingVenue/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        // POST: HearingVenue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HearingVenueId,VenueName,StreetAddress,CityId")] HearingVenue hearingVenue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hearingVenue);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "CREATE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "Hearing Venue";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }

          
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", hearingVenue.CityId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: HearingVenue/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingVenue = await _context.HearingVenues.FindAsync(id);
            if (hearingVenue == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", hearingVenue.CityId);
            return View(hearingVenue);
        }

        // POST: HearingVenue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HearingVenueId,VenueName,StreetAddress,CityId")] HearingVenue hearingVenue)
        {
            if (id != hearingVenue.HearingVenueId)
            {
                return Json(new { success = false, message = "Error! Data not found." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hearingVenue);
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "CONTENT MANAGEMENT";
                    eventLog.Category = "Hearing Venue";
                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HearingVenueExists(hearingVenue.HearingVenueId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", hearingVenue.CityId);
            return Json(new { success = true, message = "Success! Data has been updated." });
        }

        // GET: HearingVenue/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearingVenue = await _context.HearingVenues
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.HearingVenueId == id);
            if (hearingVenue == null)
            {
                return NotFound();
            }

            return View(hearingVenue);
        }

        // POST: HearingVenue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hearingVenue = await _context.HearingVenues.FindAsync(id);
            if (hearingVenue != null)
            {
                _context.HearingVenues.Remove(hearingVenue);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "Hearing Venue";
                _context.EventLogs.Add(eventLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HearingVenueExists(long id)
        {
            return _context.HearingVenues.Any(e => e.HearingVenueId == id);
        }
    }
}
