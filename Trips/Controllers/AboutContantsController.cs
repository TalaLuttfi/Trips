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
    public class AboutContantsController : Controller
    {
        private readonly DbTripsContext _context;

        public AboutContantsController(DbTripsContext context)
        {
            _context = context;
        }

        // GET: AboutContants
        public async Task<IActionResult> Index()
        {
              return _context.AboutContants != null ? 
                          View(await _context.AboutContants.ToListAsync()) :
                          Problem("Entity set 'DbTripsContext.AboutContants'  is null.");
        }

        // GET: AboutContants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AboutContants == null)
            {
                return NotFound();
            }

            var aboutContant = await _context.AboutContants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutContant == null)
            {
                return NotFound();
            }

            return View(aboutContant);
        }

        // GET: AboutContants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AboutContants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titel,Pragraph")] AboutContant aboutContant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutContant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutContant);
        }

        // GET: AboutContants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AboutContants == null)
            {
                return NotFound();
            }

            var aboutContant = await _context.AboutContants.FindAsync(id);
            if (aboutContant == null)
            {
                return NotFound();
            }
            return View(aboutContant);
        }

        // POST: AboutContants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titel,Pragraph")] AboutContant aboutContant)
        {
            if (id != aboutContant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutContant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutContantExists(aboutContant.Id))
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
            return View(aboutContant);
        }

        // GET: AboutContants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AboutContants == null)
            {
                return NotFound();
            }

            var aboutContant = await _context.AboutContants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutContant == null)
            {
                return NotFound();
            }

            return View(aboutContant);
        }

        // POST: AboutContants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AboutContants == null)
            {
                return Problem("Entity set 'DbTripsContext.AboutContants'  is null.");
            }
            var aboutContant = await _context.AboutContants.FindAsync(id);
            if (aboutContant != null)
            {
                _context.AboutContants.Remove(aboutContant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutContantExists(int id)
        {
          return (_context.AboutContants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
