using Microsoft.EntityFrameworkCore;
using Songhay.Chinook.DataAccess.Configurations;
using Songhay.Chinook.DataAccess.Models;

namespace Songhay.Chinook.DataAccess;

public class ChinookDbContext : DbContext
{
    public ChinookDbContext(DbContextOptions<ChinookDbContext> options) : base(options) { }

    public DbSet<Artist> Artists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ArtistConfiguration());
    }
}