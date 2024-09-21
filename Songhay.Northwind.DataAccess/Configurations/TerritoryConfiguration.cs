using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Songhay.Northwind.DataAccess.Models;

namespace Songhay.Northwind.DataAccess.Configurations;

public class TerritoryConfiguration : IEntityTypeConfiguration<Territory>
{
    public void Configure(EntityTypeBuilder<Territory> builder)
    {
        builder.Property(e => e.TerritoryId).HasColumnName("TerritoryID");

        builder.Property(e => e.RegionId).HasColumnName("RegionID");

        builder.HasOne(d => d.Region)
            .WithMany(p => p.Territories)
            .HasForeignKey(d => d.RegionId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
