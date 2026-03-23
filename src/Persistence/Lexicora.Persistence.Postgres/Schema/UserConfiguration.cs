using Lexicora.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicora.Persistence.Postgres.Schema;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "public", u =>
        {
            // Maybe change this to a code comment only
            u.HasComment("Represents a user of the application. Each user has a unique email and username, as well as personal information like first name, last name, and display name. The EntryCount property keeps track of how many entries the user has saved across all topics, which can be useful for displaying user summaries without needing to load all entries. Auth stuff handled by the supabase auth user schema.");
        }); // public For Supabase configuration

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(254)
            .IsUnicode()
            .IsRequired();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .HasMaxLength(30)
            .IsUnicode()
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(150)
            .IsUnicode()
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(150)
            .IsUnicode()
            .IsRequired();

        builder.Property(u => u.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(255)
            .IsUnicode();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(u => u.EntryCount)
            .HasColumnName("entry_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(u => u.TopicCount)
            .HasColumnName("topic_count")
            .HasDefaultValue(0)
            .IsRequired();

        // Defined relationships in the Topic and Entry configurations to avoid circular dependencies. Cascade delete is configured there as well.
        //builder.HasMany(u => u.Topics)
        //    .WithOne(t => t.User)
        //    .HasForeignKey(t => t.UserId)
        //    .OnDelete(DeleteBehavior.Cascade)
        //    .IsRequired();

        //builder.HasMany(u => u.Entries)
        //    .WithOne(e => e.User)
        //    .HasForeignKey(e => e.UserId)
        //    .OnDelete(DeleteBehavior.Cascade)
        //    .IsRequired();
    }
}
