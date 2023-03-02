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

            DetailVM detailVM = new()
            {
                Article = article,
                Suggestions = suggestArticle,
                Befero = before,
                After = after
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}