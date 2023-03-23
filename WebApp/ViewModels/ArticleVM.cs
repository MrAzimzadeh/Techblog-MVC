using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ArticleVM
    {
        public List<Article> Articles { get; set; }
        public Article Article { get; set; }
        public List<User> Users { get; set; }
        public User User { get; set; }
    }
}