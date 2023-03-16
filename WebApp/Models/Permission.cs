using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Permission
    {
        public class Message
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
            public string MessageText { get; set; }
            public string Status { get; set; }

            public string UserId { get; set; }
            public User User { get; set; }
        }

    }
}