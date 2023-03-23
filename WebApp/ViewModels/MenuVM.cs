using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class MenuVM
    {
        public List<Category> Category { get; set; }
        public List<Category> MenuResult { get; set; }
        public List<Article> Articles { get; set; }
    }
}