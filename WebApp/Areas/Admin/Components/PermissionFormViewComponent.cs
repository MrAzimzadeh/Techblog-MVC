using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Data;

namespace WebApp.Areas.Admin.Components
{
    [Authorize(Roles = "Admin , Admin Editor , Editor")]

    public class PermissionFormViewComponent : ViewComponent
    {
        private readonly ILogger<PermissionFormViewComponent> _logger;
        private readonly AppDbContext _context;
        public PermissionFormViewComponent(ILogger<PermissionFormViewComponent> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var conntact = _context.Permissions.OrderByDescending(x => x.DateTime).ToList().Take(10);
            return View(conntact);
            // return View(user);

        }
    }
}