﻿using System;
using System.Collections.Generic;

namespace Songhay.Northwind.DataAccess.Models
{
    public partial class OrderSubtotal
    {
        public long? OrderId { get; set; }
        public byte[]? Subtotal { get; set; }
    }
}
