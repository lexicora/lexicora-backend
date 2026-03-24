using Lexicora.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicora.Persistence.Postgres.Schema;

internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("topics", "public", t =>
        {
            // Maybe change this to a code comment only
            t.HasComment("Topics are user-defined collections of entries. They allow users to organize their saved web pages into meaningful groups. Each topic has a name, description, and tags for categorization. The EntryCount property keeps track of how many entries are in the topic, which can be useful for displaying topic summaries without needing to load all entries.");

            // Enforce max 10 tags per topic
            t.HasCheckConstraint("CK_topics_tags_max_count", "cardinality(tags) <= 10");
            t.HasCheckConstraint("CK_topics_slug_valid", "slug ~* '^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$' AND LENGTH(slug) <= 100");
        }); // public For Supabase configuration

        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Slug).IsUnique();

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(t => t.Slug)
            .HasColumnName("slug")
            .HasColumnType("citext")
            .HasMaxLength(100)
            .IsUnicode()
            .IsRequired();
        // Schema of slug:
        // {(length 87)Name.ToLowerInvariant().Replace(' ', '-')}-{ulidString[..12]/*time part + 2 random chars*/.ToLowerInvariant()}]
        // use more efficient to lower way with 0 allocation way of doing this whole thing.

        builder.Property(t => t.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsUnicode();

        //TODO: Create a gin index for this column, for querying.
        builder.Property(t => t.Tags)
            .HasColumnName("tags")
            .HasColumnType("varchar(50)[]")
            .HasDefaultValueSql("'{}'") // Default to an empty array
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.IsFavorite)
            .HasColumnName("is_favorite")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.EntryCount)
            .HasColumnName("entry_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(u => u.Topics)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Defined in the EntryConfiguration to avoid circular reference issues
        //builder.HasMany(t => t.Entries)
        //    .WithOne(e => e.Topic)
        //    .HasForeignKey(e => e.TopicId)
        //    .OnDelete(DeleteBehavior.Cascade)
        //    .IsRequired();
    }
}
