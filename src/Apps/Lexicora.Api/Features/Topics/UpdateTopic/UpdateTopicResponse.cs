using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.UpdateTopic;

public record UpdateTopicResponse(
    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid Id,

    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid UserId,
    string Slug,
    string Name,
    string? Description,
    bool IsFavorite,
    List<string> Tags,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int EntryCount
);
