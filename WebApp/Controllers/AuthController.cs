using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDTO);
            }
            var checkEmail = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (checkEmail == null)
            {
                ViewBag.Error = "This Email Is not exist! "; // 
                return View();
            }
            Microsoft
                .AspNetCore
                .Identity
                .SignInResult
                    result =
                    await _signInManger
                        .PasswordSignInAsync(
                            checkEmail,
                            loginDTO.Password,
                            loginDTO.RememberMe,
                            true
                            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
                UserName = registerDTO.Name,
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
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManger.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}