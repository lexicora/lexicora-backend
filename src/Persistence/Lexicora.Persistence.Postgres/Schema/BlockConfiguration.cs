using System.Text.Json;

using Lexicora.Domain.Constants;
using Lexicora.Domain.Entities.Block.Blocks;
using Lexicora.Persistence.Postgres.Serialization;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexicora.Persistence.Postgres.Schema;

public class BlockConfiguration : IEntityTypeConfiguration<BlockBase>
{
    public void Configure(EntityTypeBuilder<BlockBase> builder)
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

        builder.Property(b => b.Props)
            .HasColumnName("props")
            .HasColumnType("jsonb")
            .HasConversion(
                // Write to DB: Convert Dictionary to String
                dict => JsonSerializer.Serialize(dict, PersistenceJsonContext.Default.DictionaryStringJsonElement),
                // Read from DB: Convert String back to Dictionary
                json => JsonSerializer.Deserialize(json, PersistenceJsonContext.Default.DictionaryStringJsonElement) ?? new Dictionary<string, JsonElement>()
            );

        builder.Property(b => b.ContentJson)
            .HasColumnName("content_json")
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.HasDiscriminator(b => b.Type)
            .HasValue<InlineBlock>(BlockTypes.Paragraph)
            .HasValue<InlineBlock>(BlockTypes.Heading)
            .HasValue<InlineBlock>(BlockTypes.Quote)
            .HasValue<InlineBlock>(BlockTypes.Blockquote)
            .HasValue<InlineBlock>(BlockTypes.BulletListItem)
            .HasValue<InlineBlock>(BlockTypes.NumberedListItem)
            .HasValue<InlineBlock>(BlockTypes.CheckListItem)
            .HasValue<InlineBlock>(BlockTypes.ToggleListItem)
            .HasValue<InlineBlock>(BlockTypes.CodeBlock)
            .HasValue<VoidBlock>(BlockTypes.File)
            .HasValue<VoidBlock>(BlockTypes.Image)
            .HasValue<VoidBlock>(BlockTypes.Video)
            .HasValue<VoidBlock>(BlockTypes.Audio)
            .HasValue<VoidBlock>(BlockTypes.Divider)
            .HasValue<TableBlock>(BlockTypes.Table);

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
