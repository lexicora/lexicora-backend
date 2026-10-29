using System.Text.Json.Serialization;
using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Entries.CreateEntry;

public record CreateEntryResponse(
    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid Id,

    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid UserId,

    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid TopicId,

    string Slug,
    string Title,
    string? Description,
    List<string> Tags,
    bool IsFavorite,
    string? FaviconUrl,
    string Url,
    string OriginUrl,
    string? PathUrl,
    string LanguageCode,
    DateTime CreatedAt,

    // Metadata
    string? SiteName = null,
    string? Excerpt = null,
    string? Byline = null,
    DateTime? PublishedAt = null
);
