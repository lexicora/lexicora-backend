using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.GetTopicBySlug;

public record GetTopicBySlugResponse(
    [property: JsonIgnore]
    Guid Id,

    [property: JsonIgnore]
    Guid UserId,
    string Slug,
    string Name,
    string? Description,
    bool IsFavorite,
    string[] Tags,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int EntryCount
)
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);

    [JsonPropertyName("userId")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidUserId => Ulid.New(UserId);
}

// DateTime was DateTimeOffset, but changed for compatibility.
