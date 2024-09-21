﻿using System;
using System.Collections.Generic;

namespace Songhay.Northwind.DataAccess.Models
{
    public partial class CustomerAndSuppliersByCity
    {
        public string? City { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public byte[]? Relationship { get; set; }
    }
}
