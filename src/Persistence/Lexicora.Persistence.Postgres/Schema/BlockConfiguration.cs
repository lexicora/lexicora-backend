using Lexicora.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicora.Persistence.Postgres.Schema;

public class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.ToTable("blocks", "public", b =>
        {
            // Maybe change this to a code comment only
            b.HasComment("Blocks represent the content of an entry. They can be nested to create complex structures. Each block has a type that determines how it should be rendered and what properties it has. The Props column is a JSONB column that stores the properties of the block, which can vary depending on the block type. The ContentJson column is used for blocks that have content that can be represented as JSON, such as tables or code blocks.");
        }); // public For Supabase configuration

        builder.HasKey(b => b.Id);

        builder.HasIndex(b => new { b.EntryId, b.Order });

        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(b => b.EntryId)
            .HasColumnName("entry_id")
            .IsRequired();

        builder.Property(b => b.ParentBlockId)
            .HasColumnName("parent_block_id")
            .IsRequired(false);

        builder.Property(b => b.Order)
            .HasColumnName("order")
            .IsRequired();

        builder.Property(b => b.Type)
            .HasColumnName("type")
            .HasMaxLength(50) // Maybe increase later.
            .IsUnicode()
            .IsRequired();

        builder.Property(b => b.PropsJson)
            .HasColumnName("props_json")
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'{}'") // Default to an empty JSON object had: ("'{}'::jsonb")
            .IsRequired();

        builder.Property(b => b.ContentJson)
            .HasColumnName("content_json")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.HasOne(b => b.Entry)
            .WithMany(e => e.Blocks)
            .HasForeignKey(b => b.EntryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // TODO: Maybe change delete behavior to something else, but technically if a parent block is deleted in the BlockNote editor, its children get deleted to, which matches the cascading behavior.
        builder.HasOne(b => b.ParentBlock)
            .WithMany(b => b.Children)
            .HasForeignKey(b => b.ParentBlockId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}
