using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhotoUrl { get; set; }
        public string AboutAuthor { get; set; }
        public List<Article> Articles { get; set; }
        // public Comment Comment { get; set; }

        //public List<UserAds> UserAds { get; set; }
         public List<UserAds>? UserAds { get; set; }
    }
}