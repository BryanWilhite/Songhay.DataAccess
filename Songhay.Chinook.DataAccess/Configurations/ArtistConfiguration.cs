using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Songhay.Chinook.DataAccess.Models;

namespace Songhay.Chinook.DataAccess.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        // Primary Key
        builder.HasKey(t => t.ArtistId);

        // Properties
        builder.Property(t => t.Name).HasMaxLength(120);

        // Table & Column Mappings
        builder.ToTable(nameof(Artist));
        builder.Property(t => t.ArtistId).HasColumnName(nameof(Artist.ArtistId));
        builder.Property(t => t.Name).HasColumnName(nameof(Artist.Name));
    }
}