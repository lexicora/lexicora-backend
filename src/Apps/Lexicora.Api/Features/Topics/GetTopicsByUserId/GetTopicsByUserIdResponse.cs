using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Topics.GetTopicsByUserId;

public record GetTopicsByUserIdResponse
{
    [JsonIgnore]
    public Guid Id { get; init; }

    [JsonIgnore]
    public Guid UserId { get; init; }

    public required string Slug { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public bool IsFavorite { get; init; }
    public string[] Tags { get; init; } = [];
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public int EntryCount { get; init; }

    [JsonPropertyName("id")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);

    [JsonPropertyName("userId")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidUserId => Ulid.New(UserId);
}

//public record GetTopicsByUserIdResponse(
//    [property: JsonIgnore]
//    Guid Id,

//    [property: JsonIgnore]
//    Guid UserId,
//    string Slug,
//    string Name,
//    string? Description,
//    bool IsFavorite,
//    string[] Tags,
//    DateTime CreatedAt,
//    DateTime? UpdatedAt,
//    int EntryCount
//)
//{
//    [JsonPropertyName("id")]
//    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
//    public Ulid UlidId => Ulid.New(Id);

//    [JsonPropertyName("userId")]
//    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
//    public Ulid UlidUserId => Ulid.New(UserId);
//}
