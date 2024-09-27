using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasNoKey();

        builder.ToView("Invoices");

        builder.Property(e => e.CustomerId).HasColumnName("CustomerID");

        builder.Property(e => e.Freight).HasColumnType("NUMERIC");

        builder.Property(e => e.OrderDate).HasColumnType("DATETIME");

        builder.Property(e => e.OrderId).HasColumnName("OrderID");

        builder.Property(e => e.ProductId).HasColumnName("ProductID");

        builder.Property(e => e.RequiredDate).HasColumnType("DATETIME");

        builder.Property(e => e.ShippedDate).HasColumnType("DATETIME");

        builder.Property(e => e.UnitPrice).HasColumnType("NUMERIC");
    }
}
