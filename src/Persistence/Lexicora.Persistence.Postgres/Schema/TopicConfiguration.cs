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
            t.HasComment("Topics are user-defined collections of entries. They allow users to organize their saved web pages into meaningful groups. Each topic has a title, description, and tags for categorization. The EntryCount property keeps track of how many entries are in the topic, which can be useful for displaying topic summaries without needing to load all entries.");
        }); // public For Supabase configuration

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(t => t.Title)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(t => t.Tags)
            .HasColumnName("tags")
            .HasColumnType("text[]")
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.IsFavorite)
            .HasColumnName("is_favorite")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(t => t.EntryCount)
            .HasColumnName("entry_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(u => u.Topics)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(t => t.Entries)
            .WithOne(e => e.Topic)
            .HasForeignKey(e => e.TopicId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
