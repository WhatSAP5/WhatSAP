using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhatSAP.Helpers;
using WhatSAP.Models;

namespace WhatSAP.Controllers
{
    public class CartController : Controller
    {
        private readonly WhatSAPContext _context;

        public CartController(WhatSAPContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(x => x.NumOfPeople * x.Activity.Price);
            return View(cart);
        }

        public IActionResult Save(long activityId)
        {
            if (SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart") == null)
            {
                List<CartViewModel> cart = new List<CartViewModel>();
                cart.Add(new CartViewModel { NumOfPeople = 1, Activity = _context.Activity.FirstOrDefault(x => x.ActivityId == activityId) });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartViewModel> cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
                int index = IsExist(activityId);
                if(index != -1)
                {
                    cart[index].NumOfPeople++;
                }
                else
                {
                    cart.Add(new CartViewModel() { NumOfPeople = 1, Activity = _context.Activity.FirstOrDefault(x => x.ActivityId == activityId) });
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(long activityId)
        {
            List<CartViewModel> cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
            int index = IsExist(activityId);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction("Index");
        }

        private int IsExist(long id)
        {
            List<CartViewModel> cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");

            for (int i = 0; i < cart.Count; i++)
            {
                if(cart[i].Activity.ActivityId == id)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}