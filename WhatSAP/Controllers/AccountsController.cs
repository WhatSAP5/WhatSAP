using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhatSAP.Models;
using WhatSAP.Models.Account;

namespace WhatSAP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly WhatSAPContext _context;
        public AccountsController(WhatSAPContext context)
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
            return View(new LoginViewModel());
        }

        //GET /Accounts/register
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,EmailAddress,Password,ConfirmPassword,UserType")]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                switch (registerViewModel.UserType)
                {
                    case "Customer":
                        Customer customer = new Customer
                        {
                            Email = registerViewModel.EmailAddress,
                            Password = registerViewModel.Password,
                            FirstName = registerViewModel.FirstName,
                            LastName = registerViewModel.LastName
                        };
                        _context.Customer.Add(customer);
                        break;
                    case "Client":
                        Client client = new Client
                        {
                            Email = registerViewModel.EmailAddress,
                            Password = registerViewModel.Password,
                            FirstName = registerViewModel.FirstName,
                            LastName = registerViewModel.LastName
                        };
                        _context.Client.Add(client);
                        break;
                    default:
                        throw new ArgumentNullException();
                }


                await _context.SaveChangesAsync();
                ViewData["Message"] = "Registration succeed";
            }
            return View("Login");
        }
    }
}