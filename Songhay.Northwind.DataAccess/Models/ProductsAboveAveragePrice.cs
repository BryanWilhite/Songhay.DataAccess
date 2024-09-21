using System;
using System.Collections.Generic;

namespace Songhay.Northwind.DataAccess.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string? ProductName { get; set; }
        public byte[]? UnitPrice { get; set; }
    }
}
