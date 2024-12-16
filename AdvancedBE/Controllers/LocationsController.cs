
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedBE.Data;
using AdvancedBE.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedBE.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var locations = await _context.Location
                .Where(l => l.UserId == userId)
                .ToListAsync();
            return View(locations);
        }

        // GET: Locations/Details/5
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.Id == id);

            if (location == null || location.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Create([Bind("AddressLine1,AddressLine2,City,Region,ZipCode,Country,Latitude,Longitude,Landmark")] Location location)
        {
                location.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(location);
                await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Cart");

        }

        // GET: Locations/Edit/5
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null || location.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AddressLine1,AddressLine2,City,Region,ZipCode,Country,Latitude,Longitude,Landmark")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

                try
                {
                    location.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Ensure UserId remains unchanged
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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

        // GET: Locations/Delete/5
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.Id == id);

            if (location == null || location.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);
            if (location != null && location.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _context.Location.Remove(location);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.Id == id);
        }
    }
}
