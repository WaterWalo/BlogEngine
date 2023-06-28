using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogEngine.Data;
using BlogEngine.Models;

namespace BlogEngine.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly BlogContext _context;

        public ApiController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/posts
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var currentDate = DateTime.Now.Date;
            var posts = await _context.Posts
                .Where(p => p.PublicationDate <= currentDate)
                .OrderByDescending(p => p.PublicationDate)
                .ToListAsync();

            if (posts.Count == 0)
            {
                return NoContent();
            }

            return Ok(posts);
        }

        // GET: api/posts/5
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // GET: api/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories.Count == 0)
            {
                return NoContent();
            }

            return Ok(categories);
        }

        // GET: api/categories/5
        [HttpGet("categories/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/categories/5/posts
        [HttpGet("categories/{id}/posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByCategory(int id)
        {
            var currentDate = DateTime.Now.Date;
            var posts = await _context.Posts
                .Where(p => p.PublicationDate <= currentDate && p.CategorieId == id)
                .OrderByDescending(p => p.PublicationDate)
                .ToListAsync();

            if (posts.Count == 0)
            {
                return NoContent();
            }

            return Ok(posts);
        }
    }
}
