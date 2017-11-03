using Songhay.DataAccess.Tests.Domain.Models;
using Songhay.DataAccess.Tests.Domain.Repository.Mapping;
using System.Data.Entity;

namespace Songhay.DataAccess.Tests.Domain.Repository
{

    public class ChinookDbContext : DbContext
    {
        static ChinookDbContext()
        {
            Database.SetInitializer<ChinookDbContext>(null);
        }

        public ChinookDbContext()
            : base("Name=Chinook")
        {
        }

        public DbSet<Artist> Artists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArtistMap());
        }
    }
}
