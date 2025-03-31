using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TRACE.Context;
using TRACE.Helpers;
using TRACE.Models;

namespace TRACE.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ErcdbContext _context;

        public UsersController(ErcdbContext context)
        {
            _context = context;
        }

        // GET: Users
        //[Authorize(Roles = "System Admin")]
        [Route("usermanagement")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromServices] CurrentUserHelper currentUserHelper)
        {
            string userDepartment = await currentUserHelper.GetDepartmentAsync();
            var users = await _context.Users
                .Where(x => x.IsArchive == false && x.Department == userDepartment)
                .ToListAsync();

            if (users == null || !users.Any())
            {
                return Json(new { success = false, message = "No users found." });
            }

            return Json(new { success = true, data = users });
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return Json(new { success = false, message = "Email is required." });
            }

            var a = user.Email;
            // Check if a user with the same Email exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return Json(new { success = false, message = "User with the same Email already exists." });
            }

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "User created successfully!" });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }



        // GET: Users/Edit/5
        [HttpGet]
        [Route("users/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                return RedirectToAction("Logout", "External");
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [Route("users/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,UserCategory,IsEmailNotif,Fullname,Designation,Department,IsSystemNotif,IsArchive,Username")] User user)
        {
            if (id != user.Id)
            {
                return Json(new { success = false, message = "Invalid user ID." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "User updated successfully!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                {
                    return Json(new { success = false, message = "User not found." });
                }
                return Json(new { success = false, message = "A database error occurred. Please try again." });
            }
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
