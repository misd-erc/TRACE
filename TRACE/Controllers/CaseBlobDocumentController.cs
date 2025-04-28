using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.BlobStorage;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class CaseBlobDocumentController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseBlobDocumentController(ErcdbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("DocumentId,AttachmentName,Ercid,AttachmentLink,UploadedAt")] CaseBlobDocument caseBlobDocument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseBlobDocument);
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
        public async Task<IActionResult> Edit(int id, [Bind("DocumentId,AttachmentName,Ercid,AttachmentLink,UploadedAt")] CaseBlobDocument caseBlobDocument)
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
            return RedirectToAction(nameof(Index));
        }

        private bool CaseBlobDocumentExists(int id)
        {
            return _context.CaseBlobDocument.Any(e => e.DocumentId == id);
        }
    }
}
