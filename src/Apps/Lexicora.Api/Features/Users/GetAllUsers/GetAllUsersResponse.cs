using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Users.GetAllUsers;

public record GetAllUsersResponse(
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

public record GetAllUsersResponseCollection(
    [property: JsonPropertyOrder(1)]
    List<GetAllUsersResponse> Users)
{
    [JsonPropertyOrder(0)]
    public int TotalCount => Users.Count;
}
