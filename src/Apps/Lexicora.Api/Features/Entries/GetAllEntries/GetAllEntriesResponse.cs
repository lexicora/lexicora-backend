using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Entries.GetAllEntries;

/// <summary>An entry without its content blocks, for list views.</summary>
public record GetAllEntriesResponse(
    [property: JsonIgnore]
    Guid Id,

    [property: JsonIgnore]
    Guid UserId,

    [property: JsonIgnore]
    Guid TopicId,
    string Slug,
    string Title,
    string? Description,
    string[] Tags,
    bool IsFavorite,
    string? FaviconUrl,
    string Url,
    string? SiteName,
    string? Excerpt,
    DateTime CreatedAt,
    DateTime? UpdatedAt
)
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);

    [JsonPropertyName("userId")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidUserId => Ulid.New(UserId);

    [JsonPropertyName("topicId")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidTopicId => Ulid.New(TopicId);
}

public record GetAllEntriesResponseCollection(
    [property: JsonPropertyOrder(1)]
    List<GetAllEntriesResponse> Entries)
{
    [JsonPropertyOrder(0)]
    public int TotalCount => Entries.Count;
}
