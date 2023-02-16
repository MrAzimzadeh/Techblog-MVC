using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Tag 
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public List<ArticleTag> ArticleTag { get; set; }
    }
}