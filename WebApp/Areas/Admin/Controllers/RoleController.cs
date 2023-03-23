using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManaget;

        private readonly AppDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManaget, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManaget = userManaget;
            _context = context;
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
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Delete");
            }
        }
        // 
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }
            var model = new IdentityRole
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                ConcurrencyStamp = role.ConcurrencyStamp
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = model.Name;
            role.NormalizedName = model.NormalizedName;
            role.ConcurrencyStamp = model.ConcurrencyStamp;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}