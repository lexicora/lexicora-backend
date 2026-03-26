using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.GetTopicById;

public record GetTopicByIdResponse(
    [property: JsonIgnore]
    Guid Id,
    string Name,
    string? Description,
    bool IsFavorite,
    List<string> Tags,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int EntryCount
)
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);
}

// DateTime was DateTimeOffset, but changed for compatibility.
