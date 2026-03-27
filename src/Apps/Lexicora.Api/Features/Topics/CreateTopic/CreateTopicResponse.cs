using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.CreateTopic;

public record CreateTopicResponse(
    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid Id,

    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid UserId,
    string Slug,
    string Name,
    string? Description,
    List<string> Tags,
    bool IsFavorite, // Maybe leave out for this response, since always false on creation
    DateTime CreatedAt);
