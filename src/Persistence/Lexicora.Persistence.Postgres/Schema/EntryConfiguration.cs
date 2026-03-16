using Lexicora.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicora.Persistence.Postgres.Schema;

internal class EntryConfiguration : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder.ToTable("entries", "public", e =>
        {
            // Maybe change this to a code comment only
            e.HasComment("Represents a saved web page or article with metadata and content blocks.");

            // Enforce max 10 tags per entry
            e.HasCheckConstraint("CK_Tags_MaxCount", "cardinality(tags) <= 10");

            //e.HasCheckConstraint("CK_Tags_MaxMemberLength", "all_elements_len_check(tags, 50)");
            // IMPORTANT: Run following sql in the db before using this!
            /*
               CREATE OR REPLACE FUNCTION all_elements_len_check(arr text[], max_len int)
               RETURNS boolean AS $$
               BEGIN
                 RETURN NOT EXISTS (SELECT 1 FROM unnest(arr) s WHERE length(s) > max_len);
               END;
               $$ LANGUAGE plpgsql IMMUTABLE;
             */
        }); // public For Supabase configuration

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
            .HasMaxLength(500) // Maybe increase this to 1000 if needed, but 500 should be enough for most titles
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(e => e.FaviconUrl)
            .HasColumnName("favicon_url")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(e => e.Tags)
            .HasColumnName("tags")
            .HasColumnType("text[]")
            .HasDefaultValueSql("'{}'") // Default to an empty array
            .IsUnicode()
            .IsRequired();

        builder.Property(e => e.IsFavorite)
            .HasColumnName("is_favorite")
            .HasDefaultValue(false)
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
            .HasDefaultValueSql("now()")
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

        // Defined in the BlockConfiguration to avoid circular reference issues, but can be defined here if needed
        //builder.HasMany(e => e.Blocks)
        //    .WithOne(b => b.Entry)
        //    .HasForeignKey(b => b.EntryId)
        //    .OnDelete(DeleteBehavior.Cascade)
        //    .IsRequired();

        // TODO: Optimize queries with indexes if needed
    }
}
