namespace Lexicora.Api.Features.Users.CreateUser;

public record CreateUserResponse(
    Ulid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string? DisplayName,
    DateTimeOffset CreatedAt);
