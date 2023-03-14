using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public MenuViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menu = _context.Categories.ToList();
            var viewResult = View(viewName: "Default", model: menu);
            return await Task.FromResult<IViewComponentResult>(viewResult);
           
        }
    }
}