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
    public class HearingsController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public HearingsController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _currentUserHelper = currentUserHelper;
        }

        [Route("hearings")]
        public IActionResult Hearings()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHearing()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(); // Ensure connection opens

                var sql = @"
                   SELECT  
                      h.HearingID,
                      h.ERCCaseID,
	                  CC.CaseNo,
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
                  LEFT JOIN
	                  [ercdb].[cases].[ERCCases] CC ON h.ERCCaseID = CC.ERCCaseID
                  WHERE 
                      CAST(h.HearingDate AS DATE) >= CAST(GETDATE() AS DATE)

                  ORDER BY h.HearingID DESC

                    ";

                var result = await connection.QueryAsync<dynamic>(sql);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching data", error = ex.Message });
            }
        }
    }
}
