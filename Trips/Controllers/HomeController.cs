using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trips.Models;

namespace Trips.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbTripsContext _context;

        public HomeController(ILogger<HomeController> logger, DbTripsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var AboutContant = _context.AboutContants.ToList();
            var HomeContant = _context.HomeContants.ToList();
            var contact = _context.ContactUs.ToList();
            var model3 = Tuple.Create<IEnumerable<AboutContant>,IEnumerable<HomeContant>,IEnumerable<ContactU>>(AboutContant, HomeContant, contact);
            return View(model3);
            
        }

        public IActionResult User()
        {
            var AboutContant = _context.AboutContants.ToList();
            var HomeContant = _context.HomeContants.ToList();
            var model3 = Tuple.Create<IEnumerable<AboutContant>,IEnumerable<HomeContant>>(AboutContant, HomeContant);
            return View(model3);

        }

        public IActionResult Trips()
        {
            var trips = _context.Trips.ToList();

            // Your logic to get distinct destinations
            var destinations = _context.Trips.Select(t => t.Destination).Distinct().ToList();

            // Your logic to get distinct categories
            var categories = _context.Categories.Select(c => c.CategoryName).Distinct().ToList();

            // Assign the category names to each trip based on the CategoryId
            foreach (var trip in trips)
            {
                if (trip.CategoryID.HasValue)
                {
                    trip.CategoryName = categories.FirstOrDefault(c => c == trip.CategoryID.ToString()) ?? "Unknown";
                }
                else
                {
                    trip.CategoryName = "Unknown";
                }
            }

            ViewBag.Destinations = new SelectList(destinations);
            ViewBag.Categories = new SelectList(categories);

            return View(trips); // Make sure you are passing 'trips' as the model
        }

        public ActionResult Search(string searchTerm, string destination, string Category)
        {
            // Your logic to filter trips based on searchTerm, destination, and categoryName
            var filteredTrips = _context.Trips.ToList();

            if (!string.IsNullOrEmpty(destination))
            {
                filteredTrips = filteredTrips.Where(t => t.Destination == destination).ToList();
            }

            if (!string.IsNullOrEmpty(Category))
            {   
                // Find the category ID based on the provided categoryName
                var categoryId = _context.Categories
                    .Where(c => c.CategoryName == Category)
                    .Select(c => c.Id)
                    .FirstOrDefault();

                // Filter trips based on the found category ID
                if (categoryId > 0)
                {
                    filteredTrips = filteredTrips.Where(t => t.CategoryID == categoryId).ToList();
                }
                else
                {
                    // Handle the case where the category name is not found
                    // You can redirect to an error page or handle it as needed
                    return RedirectToAction("Error");
                }
            }

            // Return the filtered trips to the view
            return View(filteredTrips);
        }






        public IActionResult About()
        {
            var About = _context.AboutContants.ToList();

            return View(About);
        }


		public IActionResult Contact()
		{
            return View();
		}

		public IActionResult Book()
		{
			
			return View();
		}

        public IActionResult MoreInfo(int id)
        {
            // Retrieve the trip details from the database using the id
            var trip = _context.Trips.Find(id);

            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
           

    }
}