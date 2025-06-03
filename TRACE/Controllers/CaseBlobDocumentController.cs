using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TRACE.BlobStorage;
using TRACE.Context;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class CaseBlobDocumentController : Controller
    {
        private readonly ErcdbContext _context;
        private readonly string _connectionString;
        private readonly CurrentUserHelper _currentUserHelper;

        public CaseBlobDocumentController(ErcdbContext context, IConfiguration configuration, CurrentUserHelper currentUserHelper)
        {
            _context = context;
            _currentUserHelper = currentUserHelper;
            _connectionString = configuration.GetConnectionString("ErcDatabase");
        }

        // GET: CaseBlobDocument
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseBlobDocument.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseBlobDocumentByErcId(int id)
        {
            var documents = await _context.CaseBlobDocument
                .Where(x => x.Ercid == id)
                .ToListAsync();

            if (documents == null || documents.Count == 0)
            {
                return Json(new { success = false, message = "No documents found." });
            }

            return Json(new { success = true, data = documents });
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseBlobDocumentByModule(string module,int ercId)
        {
            List<CaseBlobDocument> documents = new List<CaseBlobDocument>();
            if (module == "Event")
            {
                 documents = await _context.CaseBlobDocument
                .Where(x => x.Module == "Event" && x.Ercid == ercId)
                .ToListAsync();
            }
            if (module == "Hearing")
            {
                documents = await _context.CaseBlobDocument
               .Where(x => x.Module == "Hearing" && x.Ercid == ercId)
               .ToListAsync();
            }
            if (module == "Milestone")
            {
                documents = await _context.CaseBlobDocument
                    .Where(x => x.Module == "Milestone" && x.Ercid == ercId && !string.IsNullOrEmpty(x.Milestone))
                    .ToListAsync();
            }


            if (documents == null || documents.Count == 0)
            {
                return Json(new { success = false, message = "No documents found." });
            }

            return Json(new { success = true, data = documents });
        }

        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            DownloadFiles downloadFiles = new DownloadFiles();
            var downloadInfo = await downloadFiles.DownloadBlobAsync(fileName);

            if (downloadInfo == null)
            {
                return NotFound("File not found in Blob Storage.");
            }

            // Return file stream
            return File(downloadInfo.Content, "application/octet-stream", Path.GetFileName(fileName));
        }

        // GET: CaseBlobDocument/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseBlobDocument = await _context.CaseBlobDocument
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (caseBlobDocument == null)
            {
                return NotFound();
            }

            return View(caseBlobDocument);
        }

        // GET: CaseBlobDocument/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseBlobDocument/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentId,AttachmentName,Ercid,AttachmentLink,UploadedAt,Module,Milestone")] CaseBlobDocument caseBlobDocument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseBlobDocument);
                EventLog eventLog = new EventLog();
                eventLog.EventDatetime = DateTime.Now;
                var currentUserName = _currentUserHelper.Email;
                var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                eventLog.UserId = user.Username;
                eventLog.Event = "CREATE";
                eventLog.Source = "CASE MANAGEMENT";
                eventLog.Category = "DOCUMENTS";
                _context.EventLogs.Add(eventLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseBlobDocument);
        }

        // GET: CaseBlobDocument/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseBlobDocument = await _context.CaseBlobDocument.FindAsync(id);
            if (caseBlobDocument == null)
            {
                return NotFound();
            }
            return View(caseBlobDocument);
        }

        // POST: CaseBlobDocument/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentId,AttachmentName,Ercid,AttachmentLink,UploadedAt,Module,Milestone")] CaseBlobDocument caseBlobDocument)
        {
            if (id != caseBlobDocument.DocumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseBlobDocument);
                    EventLog eventLog = new EventLog();
                    eventLog.EventDatetime = DateTime.Now;
                    var currentUserName = _currentUserHelper.Email;
                    var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
                    eventLog.UserId = user.Username;
                    eventLog.Event = "EDIT";
                    eventLog.Source = "CASE MANAGEMENT";
                    eventLog.Category = "DOCUMENTS";
                    _context.EventLogs.Add(eventLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseBlobDocumentExists(caseBlobDocument.DocumentId))
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
            return View(caseBlobDocument);
        }

        // GET: CaseBlobDocument/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseBlobDocument = await _context.CaseBlobDocument
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (caseBlobDocument == null)
            {
                return NotFound();
            }

            return View(caseBlobDocument);
        }

        // POST: CaseBlobDocument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseBlobDocument = await _context.CaseBlobDocument.FindAsync(id);
            if (caseBlobDocument != null)
            {
                _context.CaseBlobDocument.Remove(caseBlobDocument);
            }

            await _context.SaveChangesAsync();
            EventLog eventLog = new EventLog();
            eventLog.EventDatetime = DateTime.Now;
            var currentUserName = _currentUserHelper.Email;
            var user = _context.Users.FirstOrDefault(x => x.Email == currentUserName);
            eventLog.UserId = user.Username;
            eventLog.Event = "DELETE";
            eventLog.Source = "CASE MANAGEMENT";
            eventLog.Category = "DOCUMENTS";
            _context.EventLogs.Add(eventLog);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetBlobFoldersdac(string module, int ercId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@caseid", ercId);

                string sql;

                if (module == "Hearing" || module == "Event")
                {
                    sql = @"
                        SELECT DataId
                        FROM [ercdb].[cases].[CaseBlobDocuments]
                        WHERE ERCId = @caseid AND Module = @module
                        GROUP BY DataId";
                    parameters.Add("@module", module);
                }
                else if (module == "Milestone")
                {
                    sql = @"
                        SELECT DataId, Milestone
                        FROM [ercdb].[cases].[CaseBlobDocuments]
                        WHERE ERCId = @caseid AND Module IS NULL AND Milestone IS NOT NULL
                        GROUP BY DataId, Milestone";
                }
                else
                {
                    return Json(new { success = false, message = "Invalid module." });
                }

                var folders = (await connection.QueryAsync<CaseBlobDocument>(sql, parameters)).ToList();

                if (folders == null || folders.Count == 0)
                {
                    return Json(new { success = false, message = "No Folders found." });
                }

                return Json(new { success = true, data = folders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Error fetching folders.", error = ex.Message });
            }
        }





        private bool CaseBlobDocumentExists(int id)
        {
            return _context.CaseBlobDocument.Any(e => e.DocumentId == id);
        }
    }
}
