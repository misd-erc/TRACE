using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.BlobStorage;
using TRACE.Context;
using TRACE.DTO;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class MilestonesAchievedController : Controller
    {
        private readonly ErcdbContext _context;

        public MilestonesAchievedController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: MilestonesAchieved
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.MilestonesAchieveds.Include(m => m.CaseMilestone).Include(m => m.Erccase);
            return View(await ercdbContext.ToListAsync());
        }

        // GET: MilestonesAchieved/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestonesAchieved = await _context.MilestonesAchieveds
                .Include(m => m.CaseMilestone)
                .Include(m => m.Erccase)
                .FirstOrDefaultAsync(m => m.MilestoneAchievedId == id);
            if (milestonesAchieved == null)
            {
                return NotFound();
            }

            return View(milestonesAchieved);
        }

        // GET: MilestonesAchieved/Create
        public IActionResult Create()
        {
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "Milestone");
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId");
            return View();
        }

        // POST: MilestonesAchieved/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MilestoneAchievedId,ErccaseId,CaseMilestoneId,DatetimeAchieved,PercentAchieved")] MilestonesAchieved milestonesAchieved)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(milestonesAchieved);

                await _context.SaveChangesAsync();


                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", milestonesAchieved.CaseMilestoneId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", milestonesAchieved.ErccaseId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        [HttpPost]


        public async Task<ActionResult> PostCaseBlobDocument([FromForm] CaseBlobDocumentRequest request)
        {
            if (request.Files == null || request.Files.Length == 0)
            {
                return BadRequest("No files uploaded.");
            }

            if (!int.TryParse(request.CaseNumber, out int caseNumberParsed))
            {
                return BadRequest("Invalid CaseNumber format.");
            }

            var uploadedFiles = new List<CaseBlobDocument>();  // To store metadata for each uploaded file

            foreach (var file in request.Files)
            {
                if (file.Length == 0)
                {
                    continue; // Skip empty files
                }

                // Generate a unique file name (you can also use any other naming strategy)
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                FileUploadService fileUploadService = new FileUploadService();
                // Get a reference to the blob
                string attachmentLink = await fileUploadService.UploadDocumentFileAsync(file);

                if (string.IsNullOrEmpty(attachmentLink))
                {
                    continue; // Skip if blob upload fails
                }

                // Save the file metadata to the database
                var documentMetadata = new CaseBlobDocument
                {
                    AttachmentName = attachmentLink,
                    AttachmentLink = attachmentLink,
                    Ercid = caseNumberParsed,
                    UploadedAt = DateTime.UtcNow,
                    // Other fields from caseBlobDocument can be set here
                };

                _context.CaseBlobDocument.Add(documentMetadata);
                int result = _context.SaveChanges();
                uploadedFiles.Add(documentMetadata);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency error: {ex.Message}");
                    return Conflict("There was a concurrency error. Please try again.");
                }
            }

            // Save all the uploaded file metadata to the database
        

            // Return the metadata of all uploaded files
            return Ok(new { success = true, message = "Success! Data has been saved.", data = uploadedFiles });
        }





        // GET: MilestonesAchieved/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestonesAchieved = await _context.MilestonesAchieveds.FindAsync(id);
            if (milestonesAchieved == null)
            {
                return NotFound();
            }
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", milestonesAchieved.CaseMilestoneId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", milestonesAchieved.ErccaseId);
            return View(milestonesAchieved);
        }

        // POST: MilestonesAchieved/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("MilestoneAchievedId,ErccaseId,CaseMilestoneId,DatetimeAchieved,PercentAchieved")] MilestonesAchieved milestonesAchieved)
        {
            if (id != milestonesAchieved.MilestoneAchievedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(milestonesAchieved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilestonesAchievedExists(milestonesAchieved.MilestoneAchievedId))
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
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", milestonesAchieved.CaseMilestoneId);
            ViewData["ErccaseId"] = new SelectList(_context.Erccases, "ErccaseId", "ErccaseId", milestonesAchieved.ErccaseId);
            return View(milestonesAchieved);
        }

        // GET: MilestonesAchieved/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestonesAchieved = await _context.MilestonesAchieveds
                .Include(m => m.CaseMilestone)
                .Include(m => m.Erccase)
                .FirstOrDefaultAsync(m => m.MilestoneAchievedId == id);
            if (milestonesAchieved == null)
            {
                return NotFound();
            }

            return View(milestonesAchieved);
        }

        // POST: MilestonesAchieved/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var milestonesAchieved = await _context.MilestonesAchieveds.FindAsync(id);
            if (milestonesAchieved != null)
            {
                _context.MilestonesAchieveds.Remove(milestonesAchieved);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MilestonesAchievedExists(long id)
        {
            return _context.MilestonesAchieveds.Any(e => e.MilestoneAchievedId == id);
        }
    }
}
