using System;
using System.Collections.Generic;

namespace Songhay.Northwind.DataAccess.Models
{
    public partial class SalesTotalsByAmount
    {
        public byte[]? SaleAmount { get; set; }
        public long? OrderId { get; set; }
        public string? CompanyName { get; set; }
        public byte[]? ShippedDate { get; set; }
    }
}
