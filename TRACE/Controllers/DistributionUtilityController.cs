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
    public class DistributionUtilityController : Controller
    {
        private readonly ErcdbContext _context;

        public DistributionUtilityController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: DistributionUtility
        public async Task<IActionResult> Index()
        {
            return View(await _context.DistributionUtilities.ToListAsync());
        }

        // GET: DistributionUtility/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributionUtility = await _context.DistributionUtilities
                .FirstOrDefaultAsync(m => m.DuId == id);
            if (distributionUtility == null)
            {
                return NotFound();
            }

            return View(distributionUtility);
        }

        // GET: DistributionUtility/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DistributionUtility/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DuId,DuName,AddressLine1,AddressLine2,CityId,ZipCode,EntityCategoryId,ShortName,Region")] DistributionUtility distributionUtility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distributionUtility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distributionUtility);
        }

        // GET: DistributionUtility/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributionUtility = await _context.DistributionUtilities.FindAsync(id);
            if (distributionUtility == null)
            {
                return NotFound();
            }
            return View(distributionUtility);
        }

        // POST: DistributionUtility/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("DuId,DuName,AddressLine1,AddressLine2,CityId,ZipCode,EntityCategoryId,ShortName,Region")] DistributionUtility distributionUtility)
        {
            if (id != distributionUtility.DuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distributionUtility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributionUtilityExists(distributionUtility.DuId))
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
            return View(distributionUtility);
        }

        // GET: DistributionUtility/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distributionUtility = await _context.DistributionUtilities
                .FirstOrDefaultAsync(m => m.DuId == id);
            if (distributionUtility == null)
            {
                return NotFound();
            }

            return View(distributionUtility);
        }

        // POST: DistributionUtility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var distributionUtility = await _context.DistributionUtilities.FindAsync(id);
            if (distributionUtility != null)
            {
                _context.DistributionUtilities.Remove(distributionUtility);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributionUtilityExists(long id)
        {
            return _context.DistributionUtilities.Any(e => e.DuId == id);
        }
    }
}
