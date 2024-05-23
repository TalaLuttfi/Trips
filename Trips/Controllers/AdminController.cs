using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using Trips.Models;

namespace Trips.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbTripsContext _context;

        public AdminController(ILogger<HomeController> logger, DbTripsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {// Retrieve counts from the database
            int userCount = _context.Users.Count();
            int tripCount = _context.Trips.Count();
            int totalBookings = _context.Bookings.Count();
            int todayPayments = _context.Payments.Count(p => EF.Functions.DateDiffDay(p.DatePay, DateTime.Today) == 0);
            var trips = _context.Trips.ToList();
            // Pass counts to the view
            ViewData["UserCount"] = userCount;
            ViewData["TripCount"] = tripCount;
            ViewData["TotalBookings"] = totalBookings;
            ViewData["TodayPayments"] = todayPayments;
            return View(trips);
        }   // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fullname,Username,UPassword,PhoneNum,Gender,Imagepath,RoleId")] User user)
        {
            // Check if the retrieved user is null
            var originalUser = await _context.Users.FindAsync(id);
            if (originalUser == null)
            {
                return NotFound();
            }

            // Ensure that only one entity instance with a given key value is attached
            _context.Entry(originalUser).State = EntityState.Detached;

            // Set the role ID of the user to the original role ID
            user.RoleId = originalUser.RoleId;

            // Set the user ID to the retrieved ID
            user.Id = id;

            // Check if the retrieved user ID is null or not
            if (user.Id == null)
            {
                // Handle the case where user ID is not found in the session
                return RedirectToAction("Login", "Account"); // Redirect to login page or handle accordingly
            }
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
