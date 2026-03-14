using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.CreateUser;

public static class CreateUserMappers
{
    public static User MapToEntity(this CreateUserRequest request, Ulid ulid) => new()
    {
        Id = ulid.ToGuid(),
        Email = request.Email,
        Username = request.Username.ToLowerInvariant(),
        FirstName = request.FirstName,
        LastName = request.LastName,
        DisplayName = request.DisplayName,
        CreatedAt = DateTimeOffset.UtcNow
    };

    public static CreateUserResponse MapToResponse(this User user) => new(
        new Ulid(user.Id),
        user.Email,
        user.Username,
        user.FirstName,
        user.LastName,
        user.DisplayName,
        user.CreatedAt);
}
