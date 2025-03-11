using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TRACE.Models; // Ensure this contains the CaseDetails model

namespace TRACE.Controllers
{
    [Authorize]
    [Route("api/storeprocedure")]
    public class StoreProcedureController : Controller
    {
        private readonly ErcdbContext _context;

        public StoreProcedureController(ErcdbContext context)
        {
            _context = context;
        }

       

        [Route("CaseDetails/{id}")]
        public async Task<IActionResult> CaseDetails(long id)
        {
            var caseDetails = await _context.CaseDetails
                .FromSqlRaw("EXEC sp_rep_CaseDetails @id", new SqlParameter("@id", id))
                .AsNoTracking()
                .ToListAsync();

            if (caseDetails == null || caseDetails.Count == 0)
            {
                return NotFound();
            }

            return View(caseDetails);
        }
        [Route("LastMileCases")]
        public async Task<IActionResult> LastMileCases()
        {
            var cases = await _context.CaseLastMiles
                .FromSqlRaw("EXEC sp_rep_cases_lastmile")
                .AsNoTracking()
                .ToListAsync();

            return View(cases);
        }
    }
}
