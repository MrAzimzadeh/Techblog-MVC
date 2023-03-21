using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Data;
using WebApp.Helpers;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _env;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IHttpContextAccessor contextAccessor, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _contextAccessor = contextAccessor;
            _env = env;
        }
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
        // 
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userRoleVM = new UserRoleVM
            {
                User = user,
                Roles = _roleManager.Roles.Select(x => x.Name)
            };

            return View(userRoleVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(UserRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.User.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Kullanýcý ID'si {model.User.Id} olan kullanýcý bulunamadý.";
                return View("NotFound");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            bool hasUsersRole = userRoles.Contains("Users");

            var newRoles = new List<string>();
            if (!hasUsersRole)
            {
                newRoles.Add("Users");
            }
            else
            {
                newRoles = userRoles.Where(r => r != "Users").ToList();
            }

            if (model.Roles != null)
            {
                newRoles.AddRange(model.Roles);
            }

            var result = await _userManager.AddToRolesAsync(user, newRoles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Rol güncellenirken hata oluþtu.");
                return View("EditRole", model);
            }

            if (hasUsersRole && !newRoles.Contains("Users"))
            {
                ModelState.AddModelError("", "Kullanýcýnýn en az bir rolü olmalýdýr. Users rolü otomatik olarak atanacaktýr.");
                newRoles.Add("Users");

                result = await _userManager.AddToRolesAsync(user, newRoles);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Rol güncellenirken hata oluþtu.");
                    return View("EditRole", model);
                }
            }

            return RedirectToAction("Index");
        }



        //
        [Authorize(Roles = "Admin")]

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByIdAsync(userId);

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserInfo(string userId, string name, string surname, string email, string phoneNumber, string aboutAuthor, IFormFile Photo)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found
                return NotFound();
            }

            user.Name = name;
            user.Surname = surname;
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.AboutAuthor = aboutAuthor;
            if (Photo != null)
                user.PhotoUrl = ImageHelper.UploadSinglePhoto(Photo, _env);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // User update succeeded
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                // User update failed
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(user);
            }
        }




    }
}