﻿using System;
using System.Collections.Generic;

namespace Songhay.Northwind.DataAccess.Models
{
    public partial class OrderDetailsExtended
    {
        public long? OrderId { get; set; }
        public long? ProductId { get; set; }
        public string? ProductName { get; set; }
        public byte[]? UnitPrice { get; set; }
        public long? Quantity { get; set; }
        public double? Discount { get; set; }
        public byte[]? ExtendedPrice { get; set; }
    }
}
