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
            entry.Blocks = request.Blocks.MapToEntities(entry.Id);
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
}
