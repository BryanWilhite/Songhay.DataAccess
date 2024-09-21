using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(e => e.CustomerId).HasColumnName("CustomerID");

        builder.HasMany(d => d.CustomerTypes)
            .WithMany(p => p.Customers)
            .UsingEntity<Dictionary<string, object>>(
                "CustomerCustomerDemo",
                l => l.HasOne<CustomerDemographic>().WithMany().HasForeignKey("CustomerTypeId").OnDelete(DeleteBehavior.ClientSetNull),
                r => r.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.ClientSetNull),
                j =>
                {
                    j.HasKey("CustomerId", "CustomerTypeId");

                    j.ToTable("CustomerCustomerDemo");

                    j.IndexerProperty<string>("CustomerId").HasColumnName("CustomerID");

                    j.IndexerProperty<string>("CustomerTypeId").HasColumnName("CustomerTypeID");
                });
    }
}
