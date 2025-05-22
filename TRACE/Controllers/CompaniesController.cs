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
    public class CompaniesController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly CurrentUserHelper _currentUserHelper;

        public CompaniesController(ErcdbContext context, CurrentUserHelper currentUserHelper )
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var categories = await _context.Companies.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.Companies.Include(c => c.City).Include(c => c.EntityCategory);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.City)
                .Include(c => c.EntityCategory)
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            ViewData["EntityCategoryId"] = new SelectList(_context.EntityCategories, "EntityCategoryId", "Ecategory");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,AddressLine1,AddressLine2,CityId,ZipCode,EntityCategoryId,ShortName")] Company company)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(company);

                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);

                EventLog eventLog = new EventLog
                {
                    EventDatetime = DateTime.Now,
                    UserId = user?.Username,
                    Event = "CREATE",
                    Source = "CONTENT MANAGEMENT",
                    Category = "Company"
                };

                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Company created successfully." });
            }

            return Json(new { success = false, message = "Validation failed. Please check all fields." });
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", company.CityId);
            ViewData["EntityCategoryId"] = new SelectList(_context.EntityCategories, "EntityCategoryId", "Ecategory", company.EntityCategoryId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CompanyId,CompanyName,AddressLine1,AddressLine2,CityId,ZipCode,EntityCategoryId,ShortName")] Company company)
        {
            if (id != company.CompanyId)
            {
                return Json(new { success = false, message = "Company ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);

                    // Event log
                    EventLog eventLog = new EventLog
                    {
                        EventDatetime = DateTime.Now,
                        UserId = _currentUserHelper.Email,
                        Event = "EDIT",
                        Source = "CONTENT MANAGEMENT",
                        Category = "Company"
                    };

                    var user = _context.Users.FirstOrDefault(x => x.Email == eventLog.UserId);
                    if (user != null)
                    {
                        eventLog.UserId = user.Username;
                    }

                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Company updated successfully." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return Json(new { success = false, message = "Company no longer exists." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "A concurrency error occurred. Please try again." });
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();

            return Json(new
            {
                success = false,
                message = errors.Any() ? string.Join(" ", errors) : "Invalid data. Please check the form."
            });
        }


        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.City)
                .Include(c => c.EntityCategory)
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "DELETE";
                eventLog.Source = "ERC CASE";
                eventLog.Category = "Company";
                _context.EventLogs.Add(eventLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(long id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
