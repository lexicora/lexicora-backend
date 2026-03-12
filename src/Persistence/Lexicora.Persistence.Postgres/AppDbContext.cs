using Lexicora.Domain.Entities;
using Lexicora.Persistence.Postgres.Schema;

using Microsoft.EntityFrameworkCore;

namespace Lexicora.Persistence.Postgres;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<Block> Blocks => Set<Block>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TopicConfiguration());
        modelBuilder.ApplyConfiguration(new EntryConfiguration());
        modelBuilder.ApplyConfiguration(new BlockConfiguration());
    }
}
