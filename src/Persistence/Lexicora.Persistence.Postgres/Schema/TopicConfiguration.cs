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
            t.HasCheckConstraint("CK_Tags_MaxCount", "cardinality(tags) <= 10");

            //t.HasCheckConstraint("CK_Tags_MaxMemberLength", "all_elements_len_check(tags, 50)");
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

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(t => t.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsUnicode();

        builder.Property(t => t.Tags)
            .HasColumnName("tags")
            .HasColumnType("text[]")
            .HasDefaultValueSql("'{}'") // Default to an empty array
            .IsUnicode()
            .IsRequired();

        builder.Property(t => t.IsFavorite)
            .HasColumnName("is_favorite")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .IsRequired();

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

        // Defined in the EntryConfiguration to avoid circular reference issues
        //builder.HasMany(t => t.Entries)
        //    .WithOne(e => e.Topic)
        //    .HasForeignKey(e => e.TopicId)
        //    .OnDelete(DeleteBehavior.Cascade)
        //    .IsRequired();
    }
}
