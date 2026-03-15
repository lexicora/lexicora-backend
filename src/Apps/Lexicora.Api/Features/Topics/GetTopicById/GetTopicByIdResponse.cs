using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.GetTopicById;

public record GetTopicByIdResponse(
    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid Id,
    string Name,
    string? Description,
    bool IsFavorite,
    List<string> Tags,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    int EntryCount
);
