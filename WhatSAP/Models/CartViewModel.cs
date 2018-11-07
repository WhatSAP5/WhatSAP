using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatSAP.Models
{
    public class CartViewModel
    {
        public int NumOfPeople { get; set; }
        public double Total { get; set; }
        public List<Activity> Activities { get; set; }

        public CartViewModel()
        {
            NumOfPeople = 0;
            Total = 0;
            Activities = new List<Activity>();
        }
    }
}
