using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AdvertisementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdvertisementController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var ads = _context.Advertisements.ToList();
            return View(ads);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Advertisement advertisement, IFormFile Photo)
        {
            var path = "/uploads/" + Guid.NewGuid() + Photo.FileName;
            using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
            {
                Photo.CopyTo(fileStream);
            }


            Advertisement ads = new()
            {
                Name = advertisement.Name,
                Price = advertisement.Price,
                Rate = advertisement.Rate,
                SizeX = advertisement.SizeX,
                SizeY = advertisement.SizeY,
                DirectionAddress = "https://" + advertisement.DirectionAddress,
                CreatedDate = DateTime.Now,
                Click = 0,
                View = 0,
                PhotoUrl = path,
            };

            _context.Advertisements.Add(ads);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _context.Advertisements.FirstOrDefault(a => a.Id == id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Delete(Advertisement advertisement)
        {
            var result = _context.Advertisements.FirstOrDefault(x => x.Id == advertisement.Id);
            result.IsDeleted = true;
            _context.Advertisements.Update(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}