using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string PhotoUrl { get; set; }
        public string AboutAuthor { get; set; }

        public List<Article> Articles { get; set; }
        public List<Comment> comments { get; set; }

    }
}