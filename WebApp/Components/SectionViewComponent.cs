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
    public class SectionViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public SectionViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var photoExtensions = new[] { ".png", ".jpg", ".gif" }; // desteklenen video uzantıları
            var popularSection = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.User)
                .OrderByDescending(x => x.ViewCount)
                .AsEnumerable().Where(x => x.IsDelete == false && x.IsActive == true && photoExtensions.Any(ext => x.PhotoUrl.EndsWith(ext)))
                .Take(5)
                .ToList();
            var ads = _context.Advertisements.OrderByDescending(x=>x.CreatedDate).Where(X => X.IsDeleted == false).ToList();
            var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" }; // desteklenen video uzantıları
            var videos = _context.Articles
                .Include(x => x.Category)
                .Include(x => x.User)
                .OrderByDescending(x => x.ViewCount)
                .AsEnumerable() // sorgu sonuçlarını koleksiyona aktar
                .Where(x => x.IsDelete == false && x.IsActive == true && videoExtensions.Any(ext => x.PhotoUrl.EndsWith(ext))).Take(5).ToList();
            SectionVM sectionVm = new()
            {
                //Articles = articles,
                //PopularPost = popularPost,
                PopularSection = popularSection,
                //Advertisements = ads,
                Videos = videos,
                ADS = ads
            };
            var viewResult = View(viewName: "Default", model: sectionVm);
            return await Task.FromResult<IViewComponentResult>(viewResult);
        }
    }
}