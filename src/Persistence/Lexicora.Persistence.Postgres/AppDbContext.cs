using Lexicora.Domain.Entities;
using Lexicora.Persistence.Postgres.Schema;

using Microsoft.EntityFrameworkCore;

namespace Lexicora.Persistence.Postgres;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<Block> Blocks => Set<Block>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TopicConfiguration());
        modelBuilder.ApplyConfiguration(new EntryConfiguration());
        modelBuilder.ApplyConfiguration(new BlockConfiguration());
    }

    // Potentially override SaveChangesAsync to automatically set UpdatedAt for modified entities, but this is optional and can be handled in the application layer if preferred.
    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    var entries = ChangeTracker.Entries()
    //        .Where(e => e.State == EntityState.Modified);

    //    foreach (var entry in entries)
    //    {
    //        // If the entity has an UpdatedAt property, set it to UTC now
    //        if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
    //        {
    //            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
    //        }
    //    }

    //    return base.SaveChangesAsync(cancellationToken);
    //}
}
