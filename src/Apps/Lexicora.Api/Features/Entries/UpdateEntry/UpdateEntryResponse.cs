using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Entries.UpdateEntry;

public record UpdateEntryResponse(
    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid Id,

    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid UserId,

    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid TopicId,
    string Slug,
    string Title,
    string? Description,
    bool IsFavorite,
    List<string> Tags,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
