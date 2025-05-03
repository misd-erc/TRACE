using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    public class CaseMilestoneTemplateMemberController : Controller
    {
        private readonly ErcdbContext _context;

        public CaseMilestoneTemplateMemberController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: CaseMilestoneTemplateMember
        public async Task<IActionResult> Index()
        {
            var ercdbContext = _context.CaseMilestoneTemplateMember.Include(c => c.CaseMilestone).Include(c => c.CaseMilestoneTemplate);
            return View(await ercdbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetCaseMilestoneTemplateMember()
        {
            var categories = await _context.CaseMilestoneTemplateMember
                      .Include(m => m.CaseMilestone)
                      .Include(m => m.CaseMilestoneTemplate)
                      .Select(m => new
                      {
                          MilestoneId = m.CaseMilestone.CaseMilestoneId,  // Assuming the ID is 'CaseMilestoneId'
                          TemplateId = m.CaseMilestoneTemplate.CaseMilestoneTemplateId,  // Assuming the ID is 'CaseMilestoneTemplateId'
                          Milestone = m.CaseMilestone.Milestone,
                          TemplateName = m.CaseMilestoneTemplate.TemplateName
                      })
                      .ToListAsync();

            if (categories == null || !categories.Any())
            {
                return Json(new { success = false, message = "No categories found." });
            }

            return Json(new { success = true, data = categories });
        }

        // GET: CaseMilestoneTemplateMember/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestoneTemplateMember = await _context.CaseMilestoneTemplateMember
                .Include(c => c.CaseMilestone)
                .Include(c => c.CaseMilestoneTemplate)
                .FirstOrDefaultAsync(m => m.CaseMilestoneTemplateId == id);
            if (caseMilestoneTemplateMember == null)
            {
                return NotFound();
            }

            return View(caseMilestoneTemplateMember);
        }

        // GET: CaseMilestoneTemplateMember/Create
        public IActionResult Create()
        {
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "Milestone");
            ViewData["CaseMilestoneTemplateId"] = new SelectList(_context.CaseMilestoneTemplates, "CaseMilestoneTemplateId", "TemplateName");
            return View();
        }

        // POST: CaseMilestoneTemplateMember/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseMilestoneTemplateId,CaseMilestoneId")] CaseMilestoneTemplateMember caseMilestoneTemplateMember)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(caseMilestoneTemplateMember);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Success! Data has been saved." });
            }
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", caseMilestoneTemplateMember.CaseMilestoneId);
            ViewData["CaseMilestoneTemplateId"] = new SelectList(_context.CaseMilestoneTemplates, "CaseMilestoneTemplateId", "CaseMilestoneTemplateId", caseMilestoneTemplateMember.CaseMilestoneTemplateId);
            return Json(new { success = false, message = "Error! Please check your input." });
        }

        // GET: CaseMilestoneTemplateMember/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestoneTemplateMember = await _context.CaseMilestoneTemplateMember.FindAsync(id);
            if (caseMilestoneTemplateMember == null)
            {
                return NotFound();
            }
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", caseMilestoneTemplateMember.CaseMilestoneId);
            ViewData["CaseMilestoneTemplateId"] = new SelectList(_context.CaseMilestoneTemplates, "CaseMilestoneTemplateId", "CaseMilestoneTemplateId", caseMilestoneTemplateMember.CaseMilestoneTemplateId);
            return View(caseMilestoneTemplateMember);
        }

        // POST: CaseMilestoneTemplateMember/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CaseMilestoneTemplateId,CaseMilestoneId")] CaseMilestoneTemplateMember caseMilestoneTemplateMember)
        {
            if (id != caseMilestoneTemplateMember.CaseMilestoneTemplateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseMilestoneTemplateMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseMilestoneTemplateMemberExists(caseMilestoneTemplateMember.CaseMilestoneTemplateId))
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
            ViewData["CaseMilestoneId"] = new SelectList(_context.CaseMilestones, "CaseMilestoneId", "CaseMilestoneId", caseMilestoneTemplateMember.CaseMilestoneId);
            ViewData["CaseMilestoneTemplateId"] = new SelectList(_context.CaseMilestoneTemplates, "CaseMilestoneTemplateId", "CaseMilestoneTemplateId", caseMilestoneTemplateMember.CaseMilestoneTemplateId);
            return View(caseMilestoneTemplateMember);
        }

        // GET: CaseMilestoneTemplateMember/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseMilestoneTemplateMember = await _context.CaseMilestoneTemplateMember
                .Include(c => c.CaseMilestone)
                .Include(c => c.CaseMilestoneTemplate)
                .FirstOrDefaultAsync(m => m.CaseMilestoneTemplateId == id);
            if (caseMilestoneTemplateMember == null)
            {
                return NotFound();
            }

            return View(caseMilestoneTemplateMember);
        }

        // POST: CaseMilestoneTemplateMember/Delete/5
        [HttpPost, ActionName("DeleteMember")]
        
        public async Task<IActionResult> Delete([FromForm]  long caseMilestoneId, [FromForm] long caseMilestoneTemplateId)
        {
            // Find the CaseMilestoneTemplateMember based on both CaseMilestoneId and CaseMilestoneTemplateId
            var caseMilestoneTemplateMember = await _context.CaseMilestoneTemplateMember
                .FirstOrDefaultAsync(m => m.CaseMilestoneId == caseMilestoneId && m.CaseMilestoneTemplateId == caseMilestoneTemplateId);

            if (caseMilestoneTemplateMember == null)
            {
                return NotFound(); // If no matching record is found
            }

            // Delete the found record
            _context.CaseMilestoneTemplateMember.Remove(caseMilestoneTemplateMember);
            await _context.SaveChangesAsync(); // Save changes to the database

            return Ok();
        }

        private bool CaseMilestoneTemplateMemberExists(long id)
        {
            return _context.CaseMilestoneTemplateMember.Any(e => e.CaseMilestoneTemplateId == id);
        }
    }
}
