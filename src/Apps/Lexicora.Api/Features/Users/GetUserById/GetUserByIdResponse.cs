namespace Lexicora.Api.Features.Users.GetUserById;

public record GetUserByIdResponse(
    Ulid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string? DisplayName,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    int EntryCount,
    int TopicCount
);
