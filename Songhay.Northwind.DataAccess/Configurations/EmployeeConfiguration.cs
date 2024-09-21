using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

        builder.Property(e => e.BirthDate).HasColumnType("DATE");

        builder.Property(e => e.HireDate).HasColumnType("DATE");

        builder.HasOne(d => d.ReportsToNavigation)
            .WithMany(p => p.InverseReportsToNavigation)
            .HasForeignKey(d => d.ReportsTo);

        builder.HasMany(d => d.Territories)
            .WithMany(p => p.Employees)
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeTerritory",
                l => l.HasOne<Territory>().WithMany().HasForeignKey("TerritoryId").OnDelete(DeleteBehavior.ClientSetNull),
                r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull),
                j =>
                {
                    j.HasKey("EmployeeId", "TerritoryId");

                    j.ToTable("EmployeeTerritories");

                    j.IndexerProperty<long>("EmployeeId").HasColumnName("EmployeeID");

                    j.IndexerProperty<string>("TerritoryId").HasColumnName("TerritoryID");
                });
    }
}
