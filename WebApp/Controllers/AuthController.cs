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
            if (!ModelState.IsValid) //todo Bosluqu Yoxlayiriq
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
                            isPersistent: loginDTO.RememberMe,
                            lockoutOnFailure: true
                            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", "Email or Password is invalid!!!");
                return View();
            }
            return RedirectToAction("Index", "Home");
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
                PhotoUrl = "/uploads/",
            };
            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
            //todo =========== burada biz result eyer ugurludursa bizi Login etmis sekilde Home`a qaytaracaq @MrAzimzadeh
            if (result.Succeeded)
            {
                var test = await _userManager.AddToRoleAsync(newUser, "Users");
                await _signInManger.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerDTO);
            }
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