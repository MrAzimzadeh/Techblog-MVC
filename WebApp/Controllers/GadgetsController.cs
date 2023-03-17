using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class GadgetsController : Controller
    {
        private readonly ILogger<GadgetsController> _logger;
        private readonly AppDbContext _context;

        public GadgetsController(ILogger<GadgetsController> logger, AppDbContext context = null)
        {
            _logger = logger;
            _context = context;
        }
        #region a
        // public IActionResult Gadgets(int pg = 1)
        // {
        //     const int pageSize = 9;
        //     if (pg < 1)
        //     {
        //         pg = 1;
        //     }

        //     int articleCount = _context.Articles.Count();
        //     var pager = new Pager(articleCount, pg, pageSize);
        //     int arcSkip = (pg - 1) * pageSize;
        //     var articles = _context.Articles
        //     .Include(x => x.Category)
        //     .Include(x => x.User)
        //     .Where(x => x.IsDelete == false && x.IsActive == true && x.Category.CategoryName == "Gadgets").OrderByDescending(x => x.Id).Skip(arcSkip).Take(pager.PageSize)
        //     .ToList();
        //     ViewBag.Pager = pager;
        //     var popularPost = _context.Articles.Include(x => x.Category)
        //     .Include(x => x.User)
        //     .Where(x => x.IsDelete == false && x.IsActive == true && x.PopularPost == true).OrderByDescending(X => X.UpdatedDate).ToList();
        //     GadgetsVM gadgetsVM = new()
        //     {
        //         Articles = articles,
        //         PopularPost = popularPost
        //     };
        //     return View(gadgetsVM);
        // }
        #endregion
        public IActionResult Gadgets(int pg = 1)
        {
            const int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }

            int articleCount = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.IsDelete == false && x.IsActive == true && x.Gadgets == true || x.Category.CategoryName == "Gadgets")
                .Count();

            var pager = new Pager(articleCount, pg, pageSize);
            int arcSkip = (pg - 1) * pageSize;

            var articles = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.IsDelete == false && x.IsActive == true && x.Gadgets == true || x.Category.CategoryName == "Gadgets")
                .OrderByDescending(x => x.Id)
                .Skip(arcSkip)
                .Take(pager.PageSize)
                .ToList();

            ViewBag.Pager = pager;

            var popularPost = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.IsDelete == false && x.IsActive == true && x.PopularPost == true && x.Category.CategoryName == "Gadgets")
                .OrderByDescending(x => x.UpdatedDate)
                .ToList();

            GadgetsVM gadgetsVM = new()
            {
                Articles = articles,
                PopularPost = popularPost
            };

            return View(gadgetsVM);
        }


    }
}