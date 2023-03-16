using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Sadece rakam girin.")]
        public string Phone { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Mesaage { get; set; }
        public DateTime DateTime { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
    }
}