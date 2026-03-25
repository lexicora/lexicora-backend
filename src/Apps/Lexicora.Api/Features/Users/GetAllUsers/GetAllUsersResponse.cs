using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Features.Users.GetAllUsers;

public record GetAllUsersResponse(
    [property: JsonConverter(typeof(LowercaseUlidJsonConverter))]
    Ulid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string? DisplayName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int EntryCount,
    int TopicCount
);
