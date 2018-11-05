using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatSAP.Models;
using WhatSAP.Models.Account;

namespace WhatSAP.Controllers
{
    public class AdminController : Controller
    {
        private readonly WhatSAPContext _context;
        public AdminController(WhatSAPContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        //GET   /Accounts/login
        public IActionResult Login()
        {
            return View(new AdminLoginViewModel());
        }
        //POST /Admin/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Username, Password")]AdminLoginViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                if(adminViewModel != null)
                {
                    Administrator admin = FindAdminByUsername(adminViewModel.Username);
                    if(admin == null)
                    {
                        ModelState.AddModelError("LoginError", "Username/Password does not exist");
                        return View();
                    }
                    if (admin.Password.Equals(AccountsController.EncryptionPassword(adminViewModel.Password)))
                    {
                        HttpContext.Session.SetString("token", adminViewModel.Username);
                        HttpContext.Session.SetString("user", "Admin");
                        HttpContext.Session.SetString("userType", "Admin");

                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            ModelState.AddModelError("LoginError", "Email address/Password does not exist");
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View(new AdminRegisterViewModel());
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username, Password, ConfirmPassword")]AdminRegisterViewModel adminViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!AdminExists(adminViewModel.Username))
                {
                    Administrator admin = new Administrator
                    {
                        Username = adminViewModel.Username,
                        Password = AccountsController.EncryptionPassword(adminViewModel.Password),
                    };
                    _context.Administrator.Add(admin);
                    await _context.SaveChangesAsync();
                    ViewData["Message"] = "Registration succeed";
                    return View("Login");
                }
            }
            //TODO: Need to create UI for exception w/ message choose different email address
            throw new Exception("user email has already exist");
        }

        private Administrator FindAdminByUsername(string username)
        {
            Administrator admin;
            try
            {
                admin = _context.Administrator.SingleOrDefault(x => x.Username.Equals(username));
            }
            catch(Exception e)
            {
                admin = null;
            }
            return admin;
        }
        private bool AdminExists(string username)
        {
            bool adminValidation = _context.Administrator.Any(x => x.Username.Equals(username));

            return adminValidation;
        }
    }
}