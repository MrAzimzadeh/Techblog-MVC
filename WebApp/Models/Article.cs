using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]

        public string Content { get; set; }
        [Required]

        public string PhotoUrl { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DelateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string SeoUrl { get; set; }
        [Required]

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public List<ArticleTag> ArticleTags { get; set; }

        public string? UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}