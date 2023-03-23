using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{

    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;
        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();
            var article = _context.Articles
            .Include(x => x.User)
            .Include(x => x.Category)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Comments)
            .ThenInclude(x => x.User)
            .FirstOrDefault(x => x.Id == id);
            if (article == null)
                return NotFound();
            var cookie = _contextAccessor.HttpContext.Request.Cookies[$"Views"];
            string[] findCookie = { "" }; // ["1","5"]
            if (cookie != null)
            {
                findCookie = cookie.Split("-").ToArray();
            }
            if (!findCookie.Contains(article.Id.ToString()))
            {
                Response.Cookies.Append($"Views", $"{cookie}-{article.Id}", // 1-2
                    new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        Expires = DateTime.Now.AddMinutes(1)
                    }
                    );
                article.ViewCount += 1;
                _context.Articles.Update(article);
                _context.SaveChanges();
            }
            var suggestArticle = _context.Articles.Include(x => x.Category).Where(x => x.Id != article.Id && x.CategoryId == article.CategoryId).Take(2).ToList();
            var before = _context.Articles.FirstOrDefault(x => x.Id < id);
            var after = _context.Articles.FirstOrDefault(x => x.Id > id);

            var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" }; // desteklenen video uzantýlarý
            var videos = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.User)
                .OrderByDescending(x => x.Id)
                .AsEnumerable() // sorgu sonuçlarýný koleksiyona aktar
                .Where(x => x.IsDelete == false && x.IsActive == true && videoExtensions.Any(ext => x.PhotoUrl.EndsWith(ext))).ToList();

            DetailVM detailVM = new()
            {
                Article = article,
                Suggestions = suggestArticle,
                Befero = before,
                After = after,
                Videos = videos
            };
            return View(detailVM);
        }


        [HttpPost]
        public async Task<IActionResult> Detail(Comment comment)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Comment newComment = new();

            newComment.CreatedDate = DateTime.Now;
            newComment.UserId = userId;
            newComment.ArticleId = comment.ArticleId;
            newComment.Content = comment.Content;

            var article = _context.Articles.FirstOrDefault(x => x.Id == comment.ArticleId);
            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), new { id = article.Id, article.SeoUrl });
        }

        public async Task<IActionResult> ListArticles(string id, int pg = 1)
        {
            const int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userArt = _context.Users.FirstOrDefault(x => x.Id == userId);
            var user = await _context.Users.Include(u => u.Articles)
                                .FirstOrDefaultAsync(u => u.Id == id);
            var list = user.Articles.ToList();
            int articleCount = _context.Articles.Where(x => x.User.Id == user.Id)
                // .Where(x => x.IsDelete == false && x.IsActive == true && x.Gadgets == true || x.Category.CategoryName == "Gadgets")
                .Count();
            var pager = new Pager(articleCount, pg, pageSize);
            int arcSkip = (pg - 1) * pageSize;
            var articles = _context.Users.Include(x => x.Articles)
                .ThenInclude(x => x.Category)
                // .Include(x => x.User)
                // .Where(x => x.Articles == false && x.IsActive == true && x.Gadgets == true || x.Category.CategoryName == "Gadgets")
                .OrderByDescending(x => x.Id)
                .Skip(arcSkip)
                .Take(pager.PageSize)
                .ToList();
            ViewBag.Pager1 = pager;
            var article = _context.Articles.Where(x => x.User.Id == user.Id).Include(X => X.Category).Include(X => X.User).OrderByDescending(x=>x.CreatedDate).Skip(arcSkip)
                        .Take(pager.PageSize).ToList();
            // var popularPost = _context.Articles
            //     .Include(x => x.Category)
            //     .Include(x => x.User)
            //     .Where(x => x.IsDelete == false && x.IsActive == true && x.PopularPost == true && x.Category.CategoryName == "Gadgets")
            //     .OrderByDescending(x => x.UpdatedDate)
            //     .ToList();
            ViewBag.UserName = user.Name;
            ArticleVM articleVM = new()
            {
                Users = articles,
                Articles = article
                // PopularPost = popularPost
            };

            return View(articleVM);
        }

    }
}