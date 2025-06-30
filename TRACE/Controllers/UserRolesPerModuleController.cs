using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class UserRolesPerModuleController : Controller
    {
        private readonly ErcdbContext _context;

        public UserRolesPerModuleController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: UserRolesPerModule
        [Route("UserRolesPerModule")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserRolesPerModules.ToListAsync());
        }

        // GET: UserRolesPerModule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRolesPerModule = await _context.UserRolesPerModules
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (userRolesPerModule == null)
            {
                return NotFound();
            }

            return View(userRolesPerModule);
        }

        // GET: UserRolesPerModule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserRolesPerModule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,ModuleName,CanView,CanEdit,CanCreate,AssignedAt")] UserRolesPerModule userRolesPerModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRolesPerModule);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "User role created successfully!" });
            }

            return Json(new { success = false, message = "Invalid input. Please check the form fields." });
        }

        // GET: UserRolesPerModule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRolesPerModule = await _context.UserRolesPerModules.FindAsync(id);
            if (userRolesPerModule == null)
            {
                return NotFound();
            }
            return View(userRolesPerModule);
        }

        // POST: UserRolesPerModule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,ModuleName,CanView,CanEdit,CanCreate,AssignedAt")] UserRolesPerModule userRolesPerModule)
        {
            if (id != userRolesPerModule.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRolesPerModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRolesPerModuleExists(userRolesPerModule.RoleId))
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
            return View(userRolesPerModule);
        }

        // GET: UserRolesPerModule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRolesPerModule = await _context.UserRolesPerModules
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (userRolesPerModule == null)
            {
                return NotFound();
            }

            return View(userRolesPerModule);
        }

        // POST: UserRolesPerModule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRolesPerModule = await _context.UserRolesPerModules.FindAsync(id);
            if (userRolesPerModule != null)
            {
                _context.UserRolesPerModules.Remove(userRolesPerModule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRolesPerModuleExists(int id)
        {
            return _context.UserRolesPerModules.Any(e => e.RoleId == id);
        }
    }
}
