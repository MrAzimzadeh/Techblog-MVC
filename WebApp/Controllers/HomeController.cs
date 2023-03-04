using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(int pg = 1)
    {
        const int pageSize = 9;
        if (pg < 1)
        {
            pg = 1; 
        }
        int articleCount = _context.Articles.Count();
        var pager =  new Pager(articleCount , pg  , pageSize);
        int arcSkip = (pg -1 ) * pageSize;
        var articles = _context.Articles
        .Include(x => x.Category)
        .Include(x => x.User)
        .Where(x => x.IsDelete == false && x.IsActive == true).Skip(arcSkip).Take(pager.PageSize)
        .ToList();
        var popularPost = _context.Articles.Include(x => x.Category)
        .Include(x => x.User)
        .Where(x => x.IsDelete == false && x.IsActive == true && x.PopularPost == true).OrderByDescending(X=>X.UpdatedDate).ToList();
        HomeVM homeVM = new()
        {
            Articles = articles,
            PopularPost = popularPost
        };
        return View(homeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public override bool Equals(object? obj)
    {
        return obj is HomeController controller &&
               EqualityComparer<AppDbContext>.Default.Equals(_context, controller._context);
    }
}
