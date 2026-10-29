using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Entries.GetEntryById;

public record GetEntryByIdResponse
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
    public required string OriginUrl { get; init; }
    public string? PathUrl { get; init; }
    public required string LanguageCode { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    // Metadata
    public string? SiteName { get; init; }
    public string? Excerpt { get; init; }
    public string? Byline { get; init; }
    public DateTime? PublishedAt { get; init; }

    /// <summary>The content of the entry, as a BlockNote document.</summary>
    public List<BlockResponse> Blocks { get; set; } = [];

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

/// <summary>
/// A block in the shape of BlockNote's document format, so the editor can load it directly.
/// </summary>
public record BlockResponse
{
    [JsonIgnore]
    public Guid Id { get; init; }

    [JsonIgnore]
    public Guid? ParentBlockId { get; init; }

    [JsonPropertyOrder(1)]
    public required string Type { get; init; }

    [JsonPropertyName("props")]
    [JsonPropertyOrder(2)]
    [JsonConverter(typeof(RawJsonConverter))]
    public required string PropsJson { get; init; }

    [JsonPropertyName("content")]
    [JsonPropertyOrder(3)]
    [JsonConverter(typeof(RawJsonConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Blocks like images or dividers have no content.
    public string? ContentJson { get; init; }

    [JsonPropertyOrder(4)]
    public List<BlockResponse> Children { get; } = [];

    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);
}
