using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trips.Models;

namespace Trips.Controllers
{
    public class BookingsController : Controller
    {
        private readonly DbTripsContext _context;

        public BookingsController(DbTripsContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var dbTripsContext = _context.Bookings.Include(b => b.Trip).Include(b => b.User);
            return View(await dbTripsContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "TripName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username"); // assuming User entity has Username property
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TripId,UserId")] Booking booking, Payment payment, VisaCard visa, string email)
        {
            if (ModelState.IsValid)
            {
                // Retrieve TripId from the form data
                if (!int.TryParse(HttpContext.Request.Form["TripId"], out int tripId))
                {
                    TempData["ErrorMessage"] = "Invalid Trip ID!";
                    return RedirectToAction("Create", "Bookings");
                }

                // Retrieve the trip price from the form data
                if (!decimal.TryParse(HttpContext.Request.Form["TripPrice"], out decimal tripPrice))
                {
                    TempData["ErrorMessage"] = "Invalid Trip Price!";
                    return RedirectToAction("Create", "Bookings");
                }

                // Check if the VisaCard exists and has sufficient balance
                var existingVisa = await _context.VisaCards.FirstOrDefaultAsync(v => v.CardNum == visa.CardNum);
                if (existingVisa != null && existingVisa.Balance >= tripPrice)
                {
                    // Update VisaCard balance
                    existingVisa.Balance -= tripPrice;

                    // Use the retrieved TripId to set the TripId property of the booking
                    booking.TripId = tripId;
                    booking.UserId = HttpContext.Session.GetInt32("CustomerId");

                    // Add the booking to the context
                    _context.Add(booking);

                    // Set Payment details
                    payment.TripId = tripId;
                    payment.UserId = HttpContext.Session.GetInt32("CustomerId");
                    payment.Amount = tripPrice;
                    payment.DatePay = DateTime.Now;

                    // Add the payment to the context
                    _context.Add(payment);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    // Create invoice email body
                    string invoiceEmailBody = $@"
                <html>
                <body>
                    <h1>Invoice</h1>
                    <p>Thank you for your purchase!</p>
                    <p>Trip ID: {tripId}</p>
                    <p>Amount Paid: ${tripPrice}</p>
                    <p>Payment Date: {payment.DatePay}</p>
                </body>
                </html>";

                    // Send invoice email
                    Program.SendInvoiceEmail(email, "Your Invoice", invoiceEmailBody, tripId, tripPrice, payment.DatePay.Value);

                    // Retrieve trip details
                    var trip = await _context.Trips.FindAsync(tripId);

                    // Check if trip exists
                    if (trip != null)
                    {
                        // Create trip email body
                        string tripEmailBody = $@"
                    <html>
                    <body>
                        <h1>Trip Details</h1>
                        <p>Trip Name: {trip.TripName}</p>
                        <p>Description: {trip.TDescription}</p>
                        <p>Destination: {trip.Destination}</p>
                        <p>Start Date: {trip.StartDate:MM/dd/yyyy}</p>
                        <p>End Date: {trip.EndDate:MM/dd/yyyy}</p>
                    </body>
                    </html>";

                        // Generate PDF of trip details
                        byte[] tripPDF = Program.GenerateTripPDF(trip);

                        // Send trip email with PDF attachment
                        Program.SendTripEmail(email, "Your Trip Details", tripEmailBody, tripPDF);

                        TempData["SuccessMessage"] = "Payment successful!";
                        return RedirectToAction("Index", "Home"); // Redirect to a suitable page after payment
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Trip not found.";
                        return RedirectToAction("Index", "Home"); // Redirect to the home page if trip is not found
                    }
                }
                else
                {
                    // Handle insufficient balance or non-existing VisaCard
                    TempData["ErrorMessage"] = "Insufficient balance or invalid VisaCard!";
                    return RedirectToAction("Create", "Bookings");
                }
            }
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "TripName", booking.TripId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TripId,UserId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["TripId"] = new SelectList(_context.Trips, "Id", "TripName", booking.TripId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Trip)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'DbTripsContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
