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
    public class HomeContantsController : Controller
    {
        private readonly DbTripsContext _context;

        public HomeContantsController(DbTripsContext context)
        {
            _context = context;
        }

        // GET: HomeContants
        public async Task<IActionResult> Index()
        {
              return _context.HomeContants != null ? 
                          View(await _context.HomeContants.ToListAsync()) :
                          Problem("Entity set 'DbTripsContext.HomeContants'  is null.");
        }

        // GET: HomeContants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HomeContants == null)
            {
                return NotFound();
            }

            var homeContant = await _context.HomeContants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeContant == null)
            {
                return NotFound();
            }

            return View(homeContant);
        }

        // GET: HomeContants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomeContants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titel,Pragraph")] HomeContant homeContant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homeContant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeContant);
        }

        // GET: HomeContants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HomeContants == null)
            {
                return NotFound();
            }

            var homeContant = await _context.HomeContants.FindAsync(id);
            if (homeContant == null)
            {
                return NotFound();
            }
            return View(homeContant);
        }

        // POST: HomeContants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titel,Pragraph")] HomeContant homeContant)
        {
            if (id != homeContant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeContant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeContantExists(homeContant.Id))
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
            return View(homeContant);
        }

        // GET: HomeContants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HomeContants == null)
            {
                return NotFound();
            }

            var homeContant = await _context.HomeContants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeContant == null)
            {
                return NotFound();
            }

            return View(homeContant);
        }

        // POST: HomeContants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HomeContants == null)
            {
                return Problem("Entity set 'DbTripsContext.HomeContants'  is null.");
            }
            var homeContant = await _context.HomeContants.FindAsync(id);
            if (homeContant != null)
            {
                _context.HomeContants.Remove(homeContant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeContantExists(int id)
        {
          return (_context.HomeContants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
