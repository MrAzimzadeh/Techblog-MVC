using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    // [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManaget;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManaget)
        {
            _roleManager = roleManager;
            _userManaget = userManaget;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            if (!ModelState.IsValid)
            {
                return View(identityRole);
            }
            var checkRole = await _roleManager.FindByNameAsync(identityRole.Name);
            if (checkRole != null)
            {
                ViewBag.RoleExist = "Role is Exist ";
                return View(identityRole);
            }
            await _roleManager.CreateAsync(identityRole);

            return RedirectToAction(nameof(Index));
        }


    }
}