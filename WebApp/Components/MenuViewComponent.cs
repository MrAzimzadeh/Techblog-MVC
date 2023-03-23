using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ViewModels;

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
            var menu = _context.Articles.Include(x => x.Category).ToList();
            var categories = _context.Categories.OrderByDescending(x=>x.Id).ToList();
            var articles  = _context.Articles.Include(x=>x.Category).Where(x => x.IsDelete == false && x.IsActive == true).OrderByDescending(x=>x.Id).ToList();
            // // ViewData["CategoryList"] = viewResult;
            MenuVM menuVM = new MenuVM
            {
                Articles = articles,
                Category = categories
            };
            ViewData["menuVM"] = categories;
            var viewResult = View(viewName: "Default", model: menu);
            return await Task.FromResult<IViewComponentResult>(viewResult);

        }
    }
}