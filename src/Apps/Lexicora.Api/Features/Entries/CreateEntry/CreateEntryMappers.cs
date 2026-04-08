using Lexicora.Api.Features.Entries._Shared;
using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Entries.CreateEntry;

public static class CreateEntryMappers
{
    public static Entry MapToEntity(this CreateEntryRequest request, Guid userId, Guid topicId)
    {
        var id = Ulid.New();

        var entry = new Entry(
            id.ToGuid(),
            userId,
            topicId,
            Utilities.CreateSlug(request.Title, id.ToString()),
            request.Title.Trim(),
            request.Description?.Trim(),
            request.Tags,
            request.FaviconUrl,
            request.Url,
            request.OriginUrl,
            request.PathUrl,
            request.LanguageCode,
            request.SiteName,
            request.Excerpt,
            request.Byline,
            request.PublishedAt,
            request.IsFavorite
        );

        if (request.Blocks is { Count: > 0 })
        {
            var entryId = id.ToGuid();
            for (int i = 0; i < request.Blocks.Count; i++)
            {
                var orderGap = ( i + 1 ) * 1000.0;
                entry.Blocks.Add(request.Blocks[i].MapToEntity(entryId, null, orderGap));
            }
        }

        return entry;
    }

    public static CreateEntryResponse MapToResponse(this Entry entry) => new(
        Ulid.New(entry.Id),
        Ulid.New(entry.UserId),
        Ulid.New(entry.TopicId),
        entry.Slug,
        entry.Title,
        entry.Description,
        entry.Tags,
        entry.IsFavorite,
        entry.FaviconUrl,
        entry.Url,
        entry.OriginUrl,
        entry.PathUrl,
        entry.LanguageCode,
        entry.CreatedAt,
        entry.SiteName,
        entry.Excerpt,
        entry.Byline,
        entry.PublishedAt
    );

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
            flatBlock.PropsJson,
            flatBlock.ContentJson);

        if (flatBlock.Children.Count > 0)
        {
            for (int i = 0; i < flatBlock.Children.Count; i++)
            {
                var childOrderGap = ( i + 1 ) * 1000.0;
                block.Children.Add(flatBlock.Children[i].MapToEntity(entryId, block.Id, childOrderGap, newIdsForBlocks));
            }
        }

        return block;
    }
}
