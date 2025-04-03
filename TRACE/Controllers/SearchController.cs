using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRACE.Models;
using TRACE.Context;
using Microsoft.Extensions.Logging;

namespace TRACE.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ErcdbContext context, IConfiguration configuration, ILogger<SearchController> logger)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
            _logger = logger;
        }

        [HttpGet]
        public async Task<JsonResult> SearchModules(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                _logger.LogWarning("Empty query received.");
                return Json(new List<object>());
            }

            try
            {
                _logger.LogInformation("Received search query: {Query}", query);

                var moduleResults = modules
                    .Where(m => m.Name.ToLower().Contains(query.ToLower()))
                    .Select(m => new { Type = "Module", Name = m.Name, Link = m.Link })
                    .ToList();

                IEnumerable<dynamic> caseResults;
                using (var connection = new SqlConnection(_connectionString))
                {
                    _logger.LogInformation("Attempting database connection...");
                    await connection.OpenAsync();
                    _logger.LogInformation("Database connection successful.");

                    caseResults = await connection.QueryAsync<dynamic>(@"
                        SELECT TOP 5 ERCCaseID, CaseNo
                        FROM cases.ERCCases 
                        WHERE CaseNo LIKE @Query
                    ", new { Query = "%" + query + "%" });

                    _logger.LogInformation("Query executed successfully. Found {Count} results.", caseResults.Count());
                }

                var results = new List<object>();

                if (moduleResults.Any())
                {
                    results.AddRange(moduleResults);
                }

                if (caseResults.Any())
                {
                    results.AddRange(caseResults.Select(c => new
                    {
                        Type = "Case",
                        Name = $"<b>Case:</b> {c.CaseNo}",
                        Link = $"/casedetails?id={c.ERCCaseID}"
                    }));
                }

                _logger.LogInformation("Returning {TotalResults} results.", results.Count);
                return Json(results);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL error while executing search query.");
                return Json(new { error = "A database error occurred. Please try again later." });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in SearchModules.");
                return Json(new { error = "An unexpected error occurred. Please try again later." });
            }
        }

        private static readonly List<ModuleModel> modules = new List<ModuleModel>
        {
            new ModuleModel { Name = "<b>Case:</b> All Cases", Link = "/allcases" },
            new ModuleModel { Name = "<b>Case:</b> Letter Complaints", Link = "/lettercomplaints" },
            new ModuleModel { Name = "<b>Case:</b> Docketed Cases", Link = "/docketedcases" },
            new ModuleModel { Name = "<b>Case:</b> COC/RES", Link = "/cocres" },
            new ModuleModel { Name = "<b>Document:</b> Document Management", Link = "/documents" },
            new ModuleModel { Name = "<b>Hearing:</b> Hearings", Link = "/hearings" },
            new ModuleModel { Name = "<b>Content:</b> Content Management", Link = "/contentmanagement" },
            new ModuleModel { Name = "<b>User:</b> User Management", Link = "/usermanagement" },
            new ModuleModel { Name = "<b>Notification:</b> Notifications", Link = "/notifications" },
            new ModuleModel { Name = "<b>Settings:</b> Notification Settings", Link = "/settings" },
            new ModuleModel { Name = "<b>Settings:</b> Appearance Settings", Link = "/settings" },
            new ModuleModel { Name = "<b>Settings:</b> Theme Settings", Link = "/settings" }
        };
    }

    public class ModuleModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
