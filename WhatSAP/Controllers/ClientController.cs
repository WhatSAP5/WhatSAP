using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatSAP.Models;

namespace WhatSAP.Controllers
{
    public class ClientController : Controller
    {

        private readonly WhatSAPContext _context;

        public ClientController(WhatSAPContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(long? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var client = await _context.Client.Include(c => c.Activity).Include(c => c.Booking)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if(client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        public async Task<IActionResult> ActivityRequest(long? id)
        {

            ViewBag.ClientId = id;

            var activity = await _context.Activity
                .Include(c => c.Address)
                .Include(c => c.Category)
                .Include(c => c.Client)
                .Where(m => m.ClientId == id)
                .Where(m => m.Authorized.Equals(false))
                .ToArrayAsync();

            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }
        public ActionResult BookingList()
        {
            return View();
        }
    }
}