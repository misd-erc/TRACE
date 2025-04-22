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
    public class HearingController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public HearingController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
        }

        // GET: Hearing
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.Hearings.Include(h => h.Erccase).Include(h => h.HearingCategory).Include(h => h.HearingVenue);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetHearingByErcID(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"
            SELECT  
                h.HearingID,
                h.ERCCaseID,
                h.HearingDate,
                h.[Time],
                h.HearingVenueID,
                hv.VenueName AS HearingVenue,
                h.Remarks,
                h.HearingCategoryID,
                hc.Category AS HearingCategory,
                h.IsApproved,
                h.ApprovedBy,
                h.DatetimeApproved,
                h.OtherVenue,
                ht.HearingTypeID,
                ht.TypeOfHearing AS HearingType,
                ht.Description AS HearingTypeDescription
            FROM 
                [ercdb].[cases].[Hearings] h
            LEFT JOIN 
                [ercdb].[cases].[HearingVenues] hv ON h.HearingVenueID = hv.HearingVenueID
            LEFT JOIN 
                [ercdb].[cases].[HearingCategories] hc ON h.HearingCategoryID = hc.HearingCategoryID
            LEFT JOIN 
                [ercdb].[cases].[HearingsInHearingType] htm ON h.HearingID = htm.HearingID
            LEFT JOIN
                [ercdb].[cases].[HearingTypes] ht ON htm.HearingTypeID = ht.HearingTypeID
            WHERE 
                h.ERCCaseID = @id";

                var result = await connection.QueryAsync<dynamic>(sql, new { id });
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }


        // GET: Hearing/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearing = await _context.Hearings
                .Include(h => h.Erccase)
                .Include(h => h.HearingCategory)
                .Include(h => h.HearingVenue)
                .FirstOrDefaultAsync(m => m.HearingId == id);
            if (hearing == null)
            {
                return NotFound();
            }

            return View(hearing);
        }

        // GET: Hearing/Create
        public IActionResult Create()
        {
            
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "Category");
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "VenueName");
            ViewData["HearingTypeId"] = new SelectList(_context.HearingTypes, "HearingTypeId", "TypeOfHearing");

            ViewBag.HearingTypes = _context.HearingTypes.ToList();
            return View();
        }

        // POST: Hearing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hearing hearing, List<long> HearingTypeIDs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (hearing.IsApproved == true)
                    {
                        var currentUserName = _currentUserHelper.Email;
                        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == currentUserName);
                        hearing.ApprovedBy = user?.Username;
                        hearing.DatetimeApproved = DateTime.Now;
                    }

                    _context.Hearings.Add(hearing);
                    await _context.SaveChangesAsync();

                    if (HearingTypeIDs != null && HearingTypeIDs.Any())
                    {
                        var hearingsInHearingType = HearingTypeIDs.Select(h => new HearingsInHearingType
                        {
                            HearingID = hearing.HearingId,
                            HearingTypeID = h
                        }).ToList();

                        _context.HearingsInHearingType.AddRange(hearingsInHearingType);
                        await _context.SaveChangesAsync();
                    }

                    return Json(new { success = true, message = "Success! Hearing has been saved." });
                }

                // If model state is invalid, return validation error
                ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
                ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
                ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);
                ViewData["HearingTypes"] = await _context.HearingTypes.ToListAsync();

                return Json(new { success = false, message = "Validation error. Please check your input." });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;

                // Log it for debugging
                Console.WriteLine("CREATE ERROR: " + errorMessage);

                return Json(new { success = false, message = $"Server error: {errorMessage}" });
            }
        }





        // GET: Hearing/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearing = await _context.Hearings.FindAsync(id);
            if (hearing == null)
            {
                return NotFound();
            }
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);
            return View(hearing);
        }

        // POST: Hearing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("HearingId,ErccaseId,HearingDate,Time,HearingVenueId,Remarks,HearingCategoryId,IsApproved,ApprovedBy,DatetimeApproved,OtherVenue")] Hearing hearing)
        {
            if (id != hearing.HearingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hearing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HearingExists(hearing.HearingId))
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
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);
            return View(hearing);
        }

        // GET: Hearing/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hearing = await _context.Hearings
                .Include(h => h.Erccase)
                .Include(h => h.HearingCategory)
                .Include(h => h.HearingVenue)
                .FirstOrDefaultAsync(m => m.HearingId == id);
            if (hearing == null)
            {
                return NotFound();
            }

            return View(hearing);
        }

        // POST: Hearing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var hearing = await _context.Hearings.FindAsync(id);
            if (hearing != null)
            {
                _context.Hearings.Remove(hearing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HearingExists(long id)
        {
            return _context.Hearings.Any(e => e.HearingId == id);
        }
    }
}
