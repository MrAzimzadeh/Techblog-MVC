using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bos Gonderme Qaqaa")] // bos olmamalidi 
        [MinLength(5)]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        public List<Article>? Articles { get; set; }
    }
}