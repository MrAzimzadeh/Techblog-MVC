using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.DTOs;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManger;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManger)
        {
            _userManager = userManager;
            _signInManger = signInManger;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var checkEmail = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (checkEmail != null)
            {
                return View();
            }

            User newUser = new()
            {
                UserName = registerDTO.Email,
                Name = registerDTO.Name,
                Surname = registerDTO.Surname,
                Email = registerDTO.Email,
                AboutAuthor = "",
                PhotoUrl = "/uploads/"

            };
            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            return View();
        }
    }
}