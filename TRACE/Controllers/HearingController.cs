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
                        h.HearingLinks,
                        -- Concatenate HearingType with a comma separator, handling null values
                        STUFF((SELECT ', ' + ISNULL(ht.TypeOfHearing, '')
                               FROM [ercdb].[cases].[HearingsInHearingType] htm
                               LEFT JOIN [ercdb].[cases].[HearingTypes] ht ON htm.HearingTypeID = ht.HearingTypeID
                               WHERE htm.HearingID = h.HearingID
                               FOR XML PATH('')), 1, 2, '') AS HearingTypes,
                        -- Concatenate HearingTypeDescription with a comma separator, handling null values
                        STUFF((SELECT ', ' + ISNULL(ht.Description, '')
                               FROM [ercdb].[cases].[HearingsInHearingType] htm
                               LEFT JOIN [ercdb].[cases].[HearingTypes] ht ON htm.HearingTypeID = ht.HearingTypeID
                               WHERE htm.HearingID = h.HearingID
                               FOR XML PATH('')), 1, 2, '') AS HearingTypeDescriptions
                    FROM 
                        [ercdb].[cases].[Hearings] h
                    LEFT JOIN 
                        [ercdb].[cases].[HearingVenues] hv ON h.HearingVenueID = hv.HearingVenueID
                    LEFT JOIN 
                        [ercdb].[cases].[HearingCategories] hc ON h.HearingCategoryID = hc.HearingCategoryID
                    WHERE 
                        h.ERCCaseID = @id

                    ORDER BY h.HearingID DESC
                    ";

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

            var hearingTypes = _context.HearingTypes
                .Select(ht => new SelectListItem
                {
                    Value = ht.HearingTypeId.ToString(),
                    Text = ht.TypeOfHearing
                })
                .ToList();

            ViewBag.HearingTypes = hearingTypes; ;
            return View();
        }

        // POST: Hearing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        [Bind("HearingId,ErccaseId,HearingDate,Time,HearingVenueId,Remarks,HearingCategoryId,IsApproved,ApprovedBy,DatetimeApproved,OtherVenue, HearingLinks")] Hearing hearing,
        List<long> HearingTypes)
        {
            if (!ModelState.IsValid)
            {
                if (hearing.IsApproved == true)
                {
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    hearing.ApprovedBy = user?.Username;
                }

                _context.Hearings.Add(hearing);
                await _context.SaveChangesAsync();

                // Get the newly created HearingId
                var hearingId = hearing.HearingId;

                // Save each selected HearingType to the join table
                foreach (var hearingTypeId in HearingTypes)
                {
                    var junction = new HearingsInHearingType
                    {
                        HearingId = hearingId,
                        HearingTypeId = hearingTypeId
                    };
                    _context.HearingsInHearingTypes.Add(junction);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Success! Data has been saved." });
            }

            // Repopulate dropdowns if model is invalid
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", hearing.ErccaseId);
            ViewData["HearingCategoryId"] = new SelectList(_context.HearingCategories, "HearingCategoryId", "HearingCategoryId", hearing.HearingCategoryId);
            ViewData["HearingVenueId"] = new SelectList(_context.HearingVenues, "HearingVenueId", "HearingVenueId", hearing.HearingVenueId);

            return Json(new { success = false, message = "Error! Please check your input." });
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
