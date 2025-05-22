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
    public class CityController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CityController(ErcdbContext context, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var citirino = await _context.Cities.ToListAsync();

            if (citirino == null || !citirino.Any())
            {
                return Json(new { success = false, message = "No cities found." });
            }

            return Json(new { success = true, data = citirino });
        }

        // GET: City
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.Cities.Include(c => c.State);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: City/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: City/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = new SelectList(_context.States, "StateId", "StateName");
            return View();
        }

        // POST: City/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,CityName,StateId")] City city)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(city);

                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                EventLog eventLog = new EventLog
                {
                    EventDatetime = DateTime.Now,
                    UserId = user?.Username,
                    Event = "CREATE",
                    Source = "CONTENT MANAGEMENT",
                    Category = "CITY"
                };

                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "City created successfully." });
            }

            return Json(new { success = false, message = "Validation failed. Please check the form and try again." });
        }


        // GET: City/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(_context.States, "StateId", "StateName", city.StateId);
            return View(city);
        }

        // POST: City/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CityId,CityName,StateId")] City city)
        {
            if (id != city.CityId)
            {
                return Json(new { success = false, message = "City ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);

                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                    EventLog eventLog = new EventLog
                    {
                        EventDatetime = DateTime.Now,
                        UserId = user?.Username,
                        Event = "EDIT",
                        Source = "CONTENT MANAGEMENT",
                        Category = "CITY"
                    };

                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "City updated successfully." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
                    {
                        return Json(new { success = false, message = "City not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Json(new { success = false, message = "Validation failed. Please check the form." });
        }


        // GET: City/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "CONTENT MANAGEMENT";
                eventLog.Category = "CITY";
                _context.EventLogs.Add(eventLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(long id)
        {
            return _context.Cities.Any(e => e.CityId == id);
        }
    }
}
