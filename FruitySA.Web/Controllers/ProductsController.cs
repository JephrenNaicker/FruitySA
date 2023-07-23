using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FruitySA.Web.Data;
using FruitySA.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace FruitySA.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,ImagePath,CategoryId")] Product product, IFormFile? ImagePath)
            {
                bool pCategory = product.CategoryId != null;
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = HttpContext.User.Identity.Name;

                Random random = new Random();
                product.ProductCode = DateTime.Now.ToString("yyyyMM") + "-" + Convert.ToString(random.Next(0, 999)).PadLeft(3, '0');
                //ModelState.IsValid=true;
                if (ModelState.IsValid && pCategory)
                {
                    // Check if a file was uploaded
                    if (ImagePath != null && ImagePath.Length > 0)
                    {
                        // Save the uploaded file to a specific folder, e.g., wwwroot/Images
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                        //string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, ImagePath.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImagePath.CopyToAsync(fileStream);
                        }

                        // Set the ImagePath property of the product to the file path
                        product.ImagePath = "/Images/" + ImagePath.FileName;
                    }
                    // Add the product to the database and save changes
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("pCategory", "Select a Category, please ensure you have at least one Category created");
                }
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
                return View(product);
        }


        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            //var user= System.Web.HttpContext.Current.User.Identity.GetUserName();
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductCode,Name,Description,Price,ImagePath,CategoryId,CreatedDate,CreatedBy")] Product product, IFormFile? ImagePath)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagePath != null && ImagePath.Length > 0)
                    {
                        // Save the uploaded file to a specific folder, e.g., wwwroot/Images
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                        //string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, ImagePath.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImagePath.CopyToAsync(fileStream);
                        }

                        // Set the ImagePath property of the product to the file path
                        product.ImagePath = "/Images/" + ImagePath.FileName;
                    }
                    // Edit the product to the database and save changes
                 
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
