using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Areas.Admin.Components
{
    public class AuthViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public AuthViewComponent(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewData["user"] = user;
            var viewResult = View(viewName: "Default", model: user);
            return await Task.FromResult<IViewComponentResult>(viewResult);
            // return View(user);

        }

    }
}