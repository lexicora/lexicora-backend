using Lexicora.Domain.Entities.Block.Blocks;

using Microsoft.EntityFrameworkCore;

namespace Lexicora.Persistence.Postgres.Schema;

public class BlockConfiguration : IEntityTypeConfiguration<BlockBase>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BlockBase> builder)
    {
        builder.ToTable("blocks", "public"); // For Supabase configuration

        builder.HasKey(b => b.Id);

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

        builder.Property(b => b.ContentJson)
            .HasColumnName("content_json")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.HasOne(b => b.Entry)
            .WithMany(e => e.Blocks)
            .HasForeignKey(b => b.EntryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(b => b.ParentBlock)
            .WithMany(b => b.Children)
            .HasForeignKey(b => b.ParentBlockId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}
