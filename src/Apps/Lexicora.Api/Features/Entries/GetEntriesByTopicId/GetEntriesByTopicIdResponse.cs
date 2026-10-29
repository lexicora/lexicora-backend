using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Entries.GetEntriesByTopicId;

/// <summary>An entry without its content blocks, for list views.</summary>
/// <remarks>
/// Uses init properties instead of a positional record: results of <c>QueryMultiple</c> are not intercepted by Dapper.AOT,
/// and the reflection based fallback can't materialize positional records.
/// </remarks>
public record GetEntriesByTopicIdResponse
{
    [JsonIgnore]
    public Guid Id { get; init; }

    [JsonIgnore]
    public Guid UserId { get; init; }

    [JsonIgnore]
    public Guid TopicId { get; init; }

    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string[] Tags { get; init; } = [];
    public bool IsFavorite { get; init; }
    public string? FaviconUrl { get; init; }
    public required string Url { get; init; }
    public string? SiteName { get; init; }
    public string? Excerpt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    [JsonPropertyName("id")]
    [JsonPropertyOrder(-1)]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);

    [JsonPropertyName("userId")]
    [JsonPropertyOrder(-1)]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidUserId => Ulid.New(UserId);

    [JsonPropertyName("topicId")]
    [JsonPropertyOrder(-1)]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidTopicId => Ulid.New(TopicId);
}
