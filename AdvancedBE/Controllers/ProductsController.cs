using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvancedBE.Data;
using AdvancedBE.Models;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedBE.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexClient(int? categoryId)
        {
            // Fetch categories
            var categories = await _context.Category.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            // Fetch products, optionally filter by category
            var productsQuery = _context.Product
                .Include(p => p.Images)
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await productsQuery.ToListAsync();
            return View(products);
        }
        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Product.Include(p => p.Category);
        //    return View(await applicationDbContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Fetch categories
            var categories = await _context.Category.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            // Fetch products, optionally filter by category
            var productsQuery = _context.Product
                .Include(p => p.Images)
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Stock,CategoryId")] Product product)
        //{

        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Product product, string ImageUrl)
        //{
            
        //        // Save the product first
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();

        //        // Save the image URL to the database
        //        if (!string.IsNullOrEmpty(ImageUrl))
        //        {
        //            var image = new Image
        //            {
        //                UrlImage = ImageUrl,
        //                ProductId = product.Id
        //            };

        //            _context.Image.Add(image);
        //            await _context.SaveChangesAsync();
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Product product, IFormFile ImageUpload)
        {

                // Save the product first
                _context.Add(product);
                await _context.SaveChangesAsync();

                // Handle the image upload
                if (ImageUpload != null && ImageUpload.Length > 0)
                {
                    // Generate a unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);

                    // Define the path to save the file
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await ImageUpload.CopyToAsync(stream);
                    }

                    // Save the image URL in the database
                    var image = new Image
                    {
                        UrlImage = "/images/" + fileName,
                        ProductId = product.Id
                    };

                    _context.Image.Add(image);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
  
        }




        // GET: Products/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Stock,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

        // GET: Products/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
