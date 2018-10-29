using System;
using System.Collections.Generic;

namespace WhatSAP.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Booking = new HashSet<Booking>();
            Comment = new HashSet<Comment>();
        }

        public long CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Booking> Booking { get; set; }
        public ICollection<Comment> Comment { get; set; }
    }
}
