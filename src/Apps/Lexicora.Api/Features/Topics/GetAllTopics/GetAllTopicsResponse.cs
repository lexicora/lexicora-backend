using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.GetAllTopics;

public record GetAllTopicsResponse(
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

public record GetAllTopicsResponseCollection(
    [property: JsonPropertyOrder(1)]
    List<GetAllTopicsResponse> Topics)
{
    [JsonPropertyOrder(0)]
    public int TotalCount => Topics.Count;
}
