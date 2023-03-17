using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{

    [Area(nameof(Admin))]
    // [Authorize(Roles = "Admin,Admin Editor,Editor,Moderator")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        // private readonly HttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public DashboardController(ILogger<DashboardController> logger, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public IActionResult Index()
        {
            // todo   Article
            var articles = _context.Articles.ToList();
            var articl = _context.Articles;
            var count = articl.Count();
            ViewBag.ArticleCount = count;
            // Todo Categories
            var CategoryCount = _context.Categories.Count();
            ViewBag.CategoryCount = CategoryCount;
            // USER 
            var users = _context.Users.Count();
            ViewBag.UsersCount = users;
            var contactCount = _context.Contacts.Count();
            ViewBag.contactCount = contactCount;
            // 
            var permissions = _context.Permissions.Include(x => x.User);
            ViewBag.Message = permissions;
            DashboardVM dashboardVM = new DashboardVM
            {
                Articles = articles,

            };
            return View(dashboardVM);
        }
        [HttpPost]
        public IActionResult Index(Permission permissions)
        {
            _context.Permissions.Add(permissions);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Paylasimlarim(string id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewData["users"] = user;
            ViewData["Id"] = user.Id;

            var articles = _context.Articles.Include(x => x.User).Include(x => x.Category).Where(x => x.User.Id == user.Id).ToList();


            return View(articles);
        }

        public IActionResult PermissionForm()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewData["user"] = user;
            ViewData["UserId"] = user.Id;

            var permissions = _context.Permissions.ToList();
            return View(permissions);
        }
        [HttpPost]
        public IActionResult PermissionForm(string message , string title)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Bos = "Bos ola bilmez";
                return View();
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewData["use"] = user;
            ViewData["UId"] = user.Id;
            Permission permission = new Permission()
            {
                DateTime = DateTime.Now,
                Title = title,
                UserId = userId,
                MessageText = message,
                SenderEmail = user.Email,
                Status = false,
                SenderName = User.Identity.Name

            };
            _context.Permissions.Add(permission);
            _context.SaveChanges();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}