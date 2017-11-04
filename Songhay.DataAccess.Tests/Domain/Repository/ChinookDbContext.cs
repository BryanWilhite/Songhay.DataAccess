using Microsoft.EntityFrameworkCore;
using Songhay.DataAccess.Tests.Domain.Models;
using Songhay.DataAccess.Tests.Domain.Repository.EntityTypeConfigurations;

namespace Songhay.DataAccess.Tests.Domain.Repository
{

    public class ChinookDbContext : DbContext
    {
        public ChinookDbContext(DbContextOptions<ChinookDbContext> options) : base(options) { }

        public DbSet<Artist> Artists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfiguration(new ArtistConfiguration());
        }
    }
}
