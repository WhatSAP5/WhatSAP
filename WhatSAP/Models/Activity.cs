using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhatSAP.Models
{
    public partial class Activity
    {
        public long ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public long? CategoryId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public long? AddressId { get; set; }

        [DataType(DataType.Currency)]
        public double? Price { get; set; }
        public int? Capacity { get; set; }
        public decimal? Rate { get; set; }
        public long? ClientId { get; set; }
        public string Key { get; set; }
        public bool Authorized { get; set; }
        public long? TypeId { get; set; }
        public string RequestFormPath { get; set; }

        public Address Address { get; set; }
        public Category Category { get; set; }
        public Client Client { get; set; }
    }
}
