using BlogEngine.Data;
using BlogEngine.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext _context;

        public HomeController(BlogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                Posts = await _context.Posts.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            return View(viewModel);
        }
    }
}
