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
    [Authorize] 

    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly AppDbContext _context;

        public TagController(ILogger<TagController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var tag = _context.Tags.ToList();

            return View(tag);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            var findTag = _context.Tags.FirstOrDefault(x => x.TagName == tag.TagName);
            if (findTag != null)
            {
                ViewBag.Tagexist = "Ay qaqa bunann yazmisan !!! ";
                return View();
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var delete = _context.Tags.FirstOrDefault(x => x.Id == id);
            return View(delete);
        }
        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var edit = _context.Tags.FirstOrDefault(x=>x.Id == id);
            return View(edit);
        }
        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            var findTag = _context.Tags.FirstOrDefault(x => x.TagName == tag.TagName);
            if (findTag != null)
            {
                ViewBag.Tagexist = "Bu categorya zaten var Agzina geleni yazma!!!!";
                return View(findTag);
            }
            _context.Tags.Update(tag);
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