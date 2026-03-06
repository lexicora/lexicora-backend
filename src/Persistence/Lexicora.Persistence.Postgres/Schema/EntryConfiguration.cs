using Lexicora.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicora.Persistence.Postgres.Schema;

public class EntryConfiguration : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder.ToTable("entries", "public"); // For Supabase configuration

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.TopicId)
            .HasColumnName("topic_id")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(500)
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsUnicode();

        builder.Property(e => e.FaviconUrl)
            .HasColumnName("favicon_url")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(t => t.Tags)
            .HasColumnName("tags")
            .HasColumnType("text[]")
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.Url)
            .HasColumnName("url")
            .HasMaxLength(1000)
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.OriginUrl)
            .HasColumnName("origin_url")
            .HasMaxLength(1000)
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.PathUrl)
            .HasColumnName("path_url")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(e => e.LanguageCode)
            .HasColumnName("language_code")
            .HasMaxLength(10)
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.SiteName)
            .HasColumnName("site_name")
            .HasMaxLength(255)
            .IsUnicode();

        builder.Property(e => e.Byline)
            .HasColumnName("byline")
            .HasMaxLength(255)
            .IsUnicode();

        builder.Property(e => e.Excerpt)
            .HasColumnName("excerpt")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(e => e.PublishedAt)
            .HasColumnName("published_at");

        builder.HasOne(e => e.User)
            .WithMany(u => u.Entries)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.Topic)
            .WithMany(t => t.Entries)
            .HasForeignKey(e => e.TopicId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // TODO: Add blocks and optimize queries with indexes if needed
    }
}
