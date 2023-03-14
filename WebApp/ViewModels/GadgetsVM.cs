using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class GadgetsVM
    {
        public List<Article> Articles { get; set; }
        public List<Article> PopularPost { get; set; }
        public Article BannerArt { get; set; }
        public List<Article> SBannerArt { get; set; }
    }
}