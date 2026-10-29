using System.Text.Json;

using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Entries._Shared;

public static class FlatBlockMappers
{
    // Gaps between orders, so a block can later be moved between two others without renumbering its siblings.
    private const double OrderGap = 1000.0;

    private static readonly JsonElement EmptyProps = JsonElement.Parse("{}");

    /// <summary>
    /// Maps a list of (nested) blocks to block entities, which are attached to the given entry.
    /// </summary>
    public static List<Block> MapToEntities(this List<FlatBlock> flatBlocks, Guid entryId, Guid? parentId = null, bool newIdsForBlocks = true)
    {
        var blocks = new List<Block>(flatBlocks.Count);
        for (int i = 0; i < flatBlocks.Count; i++)
        {
            blocks.Add(flatBlocks[i].MapToEntity(entryId, parentId, ( i + 1 ) * OrderGap, newIdsForBlocks));
        }

        return blocks;
    }

    public static Block MapToEntity(this FlatBlock flatBlock, Guid entryId, Guid? parentId, double orderIndex, bool newIdsForBlocks = true)
    {
        var blockId = newIdsForBlocks
            ? Ulid.New(UlidConfigurations.BatchOptions).ToGuid()
            : Guid.TryParse(flatBlock.Id, out var directGuid)
                ? directGuid
                : Ulid.TryParseStrict(flatBlock.Id, out var ulid)
                    ? ulid.ToGuid()
                    : Ulid.New(UlidConfigurations.BatchOptions).ToGuid(); // Fallback if invalid ID passed

        var block = new Block(
            blockId,
            entryId,
            parentId,
            flatBlock.Order ?? orderIndex,
            flatBlock.Type,
            flatBlock.PropsJson.ValueKind == JsonValueKind.Object ? flatBlock.PropsJson : EmptyProps,
            flatBlock.ContentJson);

        if (flatBlock.Children.Count > 0)
        {
            block.Children = flatBlock.Children.MapToEntities(entryId, block.Id, newIdsForBlocks);
        }

        return block;
    }
}
