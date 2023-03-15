using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class HomeVM
    {
        public List<Article> Articles { get; set; }
        public List<Article> PopularPost { get; set; }
        public List<Article> PopularSection { get; set; }
        public Article BannerArt { get; set; }
        public List<Article> SBannerArt { get; set; }
        public List<Advertisement> Advertisements { get; set; }
    }
}