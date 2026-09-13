using Microsoft.EntityFrameworkCore;

namespace Nokubico.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets are optional when using ApplyConfigurationsFromAssembly, but can be explicit for clarity
        public DbSet<Domain.Entities.User> Users => Set<Domain.Entities.User>();
        public DbSet<Domain.Entities.Account> Accounts => Set<Domain.Entities.Account>();
        public DbSet<Domain.Entities.Session> Sessions => Set<Domain.Entities.Session>();
        public DbSet<Domain.Entities.Product> Products => Set<Domain.Entities.Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all IEntityTypeConfiguration implementations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
