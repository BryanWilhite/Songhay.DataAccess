using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(e => e.OrderId).HasColumnName("OrderID");

        builder.Property(e => e.CustomerId).HasColumnName("CustomerID");

        builder.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

        builder.Property(e => e.Freight)
            .HasColumnType("NUMERIC")
            .HasDefaultValueSql("0");

        builder.Property(e => e.OrderDate).HasColumnType("DATETIME");

        builder.Property(e => e.RequiredDate).HasColumnType("DATETIME");

        builder.Property(e => e.ShippedDate).HasColumnType("DATETIME");

        builder.HasOne(d => d.Customer)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.CustomerId);

        builder.HasOne(d => d.Employee)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.EmployeeId);

        builder.HasOne(d => d.ShipViaNavigation)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.ShipVia);
    }
}
