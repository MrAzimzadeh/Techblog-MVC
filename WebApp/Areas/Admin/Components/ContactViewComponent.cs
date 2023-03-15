using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Areas.Admin.Components
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ContactViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var conntact = _context.Contacts.OrderByDescending(x=>x.Id).ToList();
            return View(conntact);
        }
    }
}