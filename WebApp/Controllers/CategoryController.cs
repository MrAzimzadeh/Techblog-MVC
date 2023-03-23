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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            var category = _context.Categories
                .Include(c => c.Articles)
                .FirstOrDefault(c => c.Id == id);
            ViewData["CategoryName"] = category.CategoryName;
            if (category == null)
            {
                return NotFound();
            }

            return View(category.Articles);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}