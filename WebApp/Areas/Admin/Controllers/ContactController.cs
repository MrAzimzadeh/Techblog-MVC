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
    [Authorize(Roles = "Admin , Admin Editor , Editor")]
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly AppDbContext _context;
        public ContactController(ILogger<ContactController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var contact = _context.Contacts.OrderByDescending(x => x.Id).ToList();
            return View(contact);
        }

        public IActionResult Delete(int id)
        {
            var data = _context.Contacts.FirstOrDefault(a => a.Id == id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Delete(Contact contact)
        {
            var result = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
            result.IsDelete = true;
            _context.Contacts.Update(result);
            _context.SaveChanges(); 
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var result = _context.Contacts.FirstOrDefault(x => x.Id ==id);
            return View(result  );
        }
        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (contact.Status == true)
            {
                contact.IsDelete = true;
            }
            _context.Contacts.Update(contact);
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