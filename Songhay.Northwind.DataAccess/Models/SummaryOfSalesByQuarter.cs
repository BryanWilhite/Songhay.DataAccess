using System;
using System.Collections.Generic;

namespace Songhay.Northwind.DataAccess.Models
{
    public partial class SummaryOfSalesByQuarter
    {
        public byte[]? ShippedDate { get; set; }
        public long? OrderId { get; set; }
        public byte[]? Subtotal { get; set; }
    }
}
