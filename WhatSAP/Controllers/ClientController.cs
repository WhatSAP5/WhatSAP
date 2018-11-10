using System;
using System.Collections.Generic;
using System.IO;
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

        public IActionResult RequestForm()
        {
            Activity activity = new Activity();
            return View(activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestForm([Bind("ActivityId", "ActivityName", "Description", "ClientId")]Activity activity, IFormFile file)
        {
            if (file == null)
                return Content("File is not selected");

            var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot/RequestForm",
                                    file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (ModelState.IsValid)
            {
                activity.RequestFormPath = Path.Combine("wwwroot/RequestForm", file.FileName);
                activity.ClientId = HttpContext.Session.GetInt32("token");
                _context.Activity.Add(activity);
            }
            await _context.SaveChangesAsync();

            return View();
        }

        public ActionResult BookingList()
        {
            return View();
        }


    }
}