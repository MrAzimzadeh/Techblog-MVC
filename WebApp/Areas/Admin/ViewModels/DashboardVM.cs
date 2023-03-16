using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Admin.ViewModels
{
    public class DashboardVM
    {
        public List<Article> Articles { get; set; }
        public Article Article { get; set; }
        public List<Category> Categories { get; set; }
        public Category Category { get; set; }
    }
}