using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Advertisement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public int View { get; set; }
        public int Click { get; set; }
        public string PhotoUrl { get; set; }
        public string DirectionAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}