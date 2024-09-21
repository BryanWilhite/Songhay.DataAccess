using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(e => e.ProductId).HasColumnName("ProductID");

        builder.Property(e => e.CategoryId).HasColumnName("CategoryID");

        builder.Property(e => e.Discontinued).HasDefaultValueSql("'0'");

        builder.Property(e => e.ReorderLevel).HasDefaultValueSql("0");

        builder.Property(e => e.SupplierId).HasColumnName("SupplierID");

        builder.Property(e => e.UnitPrice)
            .HasColumnType("NUMERIC")
            .HasDefaultValueSql("0");

        builder.Property(e => e.UnitsInStock).HasDefaultValueSql("0");

        builder.Property(e => e.UnitsOnOrder).HasDefaultValueSql("0");

        builder.HasOne(d => d.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(d => d.CategoryId);

        builder.HasOne(d => d.Supplier)
            .WithMany(p => p.Products)
            .HasForeignKey(d => d.SupplierId);
    }
}
