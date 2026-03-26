using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Users.GetUserByUsername;

public record GetUserByUsernameResponse(
    [property: JsonIgnore]
    Guid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string? DisplayName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int EntryCount,
    int TopicCount
)
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(LowercaseUlidJsonConverter))]
    public Ulid UlidId => Ulid.New(Id);
}
