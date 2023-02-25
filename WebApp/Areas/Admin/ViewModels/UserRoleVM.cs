using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Admin.ViewModels
{
    public class UserRoleVM
    {
        public User User{ get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}