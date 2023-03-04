using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;

namespace WebApp.Controllers
{

    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public TagController(ILogger<CategoryController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            var tags = _context.Tags
                .Include(c => c.ArticleTag)
                .ThenInclude(x=>x.Article)
                .FirstOrDefault(c => c.Id == id);
            ViewData["TagName"] = tags.TagName;
            ViewData["TagId"] = tags.Id;

            // var categorya = _context.Articles.Include(x=>x.Category);
            // ViewData["CatID"] = categorya.Category.Id;
            // ViewData["CatName"] = categorya.Category.CategoryName;  
            if (tags == null)
            {
                return NotFound();
            }

            return View(tags.ArticleTag);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}