using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public async Task<IActionResult> AddRole(string id)
        {
            if (id == null) return NotFound();
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList(); //! olan rollari yoxlamaq ucun
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();
            UserRoleVM userRoleVM = new()
            {
                User = user,
                Roles = roles.Except(userRoles) //! olan rollari cixarmaq ucun bunu edirik 
            };
            return View(userRoleVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string id, string role)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)   
            {
                return NotFound();
            }
            var userAddRole = await _userManager.AddToRoleAsync(user, role);
            if (!userAddRole.Succeeded)     
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ListArticles(string id)
        {
            var user = await _context.Users.Include(u => u.Articles)
                                            .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user.Articles);
        }


    }
}