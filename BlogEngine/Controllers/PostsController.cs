using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Data;
using BlogEngine.Models;

namespace BlogEngine.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList(); // Fetch categories from the database and pass them to the view
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PublicationDate,Content,CategorieId")] Post post)
        {
            bool isInvalid = false;
            if (!ValidateUniquePostTitle(post))
            {
                ModelState.AddModelError("Title", "The Title must be unique.");
                isInvalid = true;
            }

            if (ModelState.IsValid && !isInvalid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();  
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Categories = _context.Categories.ToList(); // Fetch categories from the database and pass them to the view
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            ViewBag.Categories = _context.Categories.ToList(); // Fetch categories from the database and pass them to the view

            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,PublicationDate,Content,CategorieId")] Post post)
        {
            bool isInvalid = false;
            if (id != post.Id)
            {
                return NotFound();
            }
            if (!ValidateUniquePostTitle(post))
            {
                ModelState.AddModelError("Title", "The Title must be unique.");
                isInvalid = true;
            }

            if (ModelState.IsValid && !isInvalid)
            {
                try
                {
                    var existingPost = await _context.Posts.FindAsync(post.Id);
                    if (existingPost != null)
                    {
                        // Making a copy to ensure that the context is not already tracked.
                        existingPost.Title = post.Title;
                        existingPost.CategorieId = post.CategorieId;
                        existingPost.Content = post.Content;
                        existingPost.PublicationDate = post.PublicationDate;
                        _context.Update(existingPost);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            ViewBag.Categories = _context.Categories.ToList(); // Fetch categories from the database and pass them to the view
            return View(post);
        }

        private bool PostExists(long id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        private bool ValidateUniquePostTitle(Post post)
        {
            var existingPost = _context.Posts.FirstOrDefaultAsync(p => p.Title == post.Title);
            if (existingPost.Result != null)
            {
                if (existingPost.Result.Id != post.Id)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
