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
    
    public class VideoController : Controller
    {
        private AppDbContext _context;

        public VideoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
        var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" }; // desteklenen video uzantıları
        var videos = _context.Articles
            .Include(x => x.Category)
            .Include(x => x.User)
            .OrderByDescending(x => x.UpdatedDate)
            .AsEnumerable() // sorgu sonuçlarını koleksiyona aktar
            .Where(x => x.IsDelete == false && x.IsActive == true && videoExtensions.Any(ext => x.PhotoUrl.EndsWith(ext))).ToList();
                return View(videos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}