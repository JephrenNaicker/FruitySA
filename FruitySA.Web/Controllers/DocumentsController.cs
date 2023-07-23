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
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Documents
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.Documents != null ? 
                          View(await _context.Documents.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Documents'  is null.");
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

   
        [Authorize]
        [HttpGet]
        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(Document documents, IFormFile ExcelDocumentPath)
        {//this needs a get and post
            if (ExcelDocumentPath != null && ExcelDocumentPath.Length > 0)
            {

                if (ExcelDocumentPath.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    
                    documents.ExcelDateupload = DateTime.Now;
                    documents.UserUploaded = HttpContext.User.Identity.Name;
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents");
                    string filePath = Path.Combine(uploadsFolder, ExcelDocumentPath.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ExcelDocumentPath.CopyToAsync(fileStream);
                    }
                    // Set the ImagePath property of the product to the file path
                    documents.ExcelDocumentPath = "/Documents/" + ExcelDocumentPath.FileName;
                    // Add the product to the database and save changes
                    //check the created date
                    _context.Documents.Add(documents);
                    await _context.SaveChangesAsync();
                    ViewBag.UploadStatus = "Excel File uploaded Successfully";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ModelState.AddModelError("xlsxDocumentPath", "Please upload Excel only,Confirm your file before retrying");


                    return View();
                }

            }
            return View();
        }


        public IActionResult Download(int id)
        {
            // Retrieve the document by its ID from the database
            var document = _context.Documents.Find(id);

            if (document == null)
            {
                return NotFound();
            }

            // Get the physical path to the document file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", document.ExcelDocumentPath.TrimStart('/'));

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Return the file as a FileStreamResult with the appropriate content type
                return File(new FileStream(filePath, FileMode.Open, FileAccess.Read), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", document.ExcelDocumentPath);
            }
            else
            {
                // If the file is not found, return a NotFound response
                return NotFound();
            }
        }


        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
          return (_context.Documents?.Any(e => e.DocumentId == id)).GetValueOrDefault();
        }
    }
}
