using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class UserAds
    {
        public int Id { get; set; }
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Price { get; set; }
    }
}