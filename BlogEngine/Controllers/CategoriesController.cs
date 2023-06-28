using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Data;
using BlogEngine.Models;

namespace BlogEngine.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly BlogContext _context;

        public CategoriesController(BlogContext context)
        {
            _context = context;
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Category category)
        {
            bool isInvalid = false;
            if (!ValidateUniqueCategoryTitle(category))
            {
                ModelState.AddModelError("Title", "The Title must be unique.");
                isInvalid = true;
            }

            if (ModelState.IsValid && !isInvalid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Category category)
        {
            bool isInvalid = false;
            if (id != category.Id)
            {
                return NotFound();
            }
            if (!ValidateUniqueCategoryTitle(category))
            {
                ModelState.AddModelError("Title", "The Title must be unique.");
                isInvalid = true;
            }

            if (ModelState.IsValid && !isInvalid)
            {
                try
                {
                    var existingCategory = await _context.Categories.FindAsync(category.Id);
                    if (existingCategory != null)
                    {
                        existingCategory.Title = category.Title; // Update the existing category instead of attaching a new one
                        _context.Update(existingCategory);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        private bool ValidateUniqueCategoryTitle(Category category)
        {
            var existingCategory = _context.Categories.FirstOrDefaultAsync(c => c.Title == category.Title);
            if (existingCategory.Result != null)
            {
                if (existingCategory.Result.Id != category.Id)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
