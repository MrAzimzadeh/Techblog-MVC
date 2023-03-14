using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class GadgetsController : Controller
    {
        private readonly ILogger<GadgetsController> _logger;
        private readonly AppDbContext _context;

        public GadgetsController(AppDbContext context, ILogger<GadgetsController> logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var article = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.ArticleTags)
                .ThenInclude(x => x.Tag)
                .OrderByDescending(x=>x.Id)
                .ToList();
            return View(article);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}