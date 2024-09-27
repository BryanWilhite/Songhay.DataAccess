using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => new { e.OrderId, e.ProductId });

        builder.ToTable("Order Details");

        builder.Property(e => e.OrderId).HasColumnName("OrderID");

        builder.Property(e => e.ProductId).HasColumnName("ProductID");

        builder.Property(e => e.Quantity).HasDefaultValueSql("1");

        builder.Property(e => e.UnitPrice)
            .HasColumnType("NUMERIC")
            .HasDefaultValueSql("0");

        builder.HasOne(d => d.Order)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
