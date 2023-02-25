using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    // [Authorize(Roles ="Admin, Admin Editor")] 
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var category = _context.Categories.ToList();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var findCategory = _context.Categories.FirstOrDefault(x => x.CategoryName == category.CategoryName); //  Eyni adda bir categorynin olub olmamasini yoxlayir 
            // biz burada sert veririk eger ki category ile eyi adda var sa 
            // viev bag le mesaj gonderirik ki bes bu adda senin bu adda bir categorin var 
            if (findCategory != null)
            {
                ViewBag.CategoryExist = "this Category is exist ";
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var findCategory = _context.Categories.FirstOrDefault(x => x.CategoryName == category.CategoryName);
            if (findCategory != null)
            {
                ViewBag.CategoryExist = "Bu categorya zaten var Agzina geleni yazma!!!!";
                return View(findCategory);
            }
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            var delete = _context.Categories.FirstOrDefault(x => x.Id == Id);
            return View(delete);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}