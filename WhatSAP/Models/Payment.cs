using System;
using System.Collections.Generic;

namespace WhatSAP.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Booking = new HashSet<Booking>();
        }

        public long PaymentId { get; set; }
        public bool PaymentState { get; set; }
        public DateTime? PaymentDate { get; set; }

        public ICollection<Booking> Booking { get; set; }
    }
}
