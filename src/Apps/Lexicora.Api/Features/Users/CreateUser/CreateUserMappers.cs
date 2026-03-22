using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.CreateUser;

public static class CreateUserMappers
{
    public static User MapToEntity(this CreateUserRequest request /*Ulid ulid*/) => new()
    {
        Id = Ulid.New().ToGuid(),
        Email = request.Email.Trim(),
        Username = request.Username.ToLowerInvariant(),
        FirstName = request.FirstName.Trim(),
        LastName = request.LastName.Trim(),
        DisplayName = request.DisplayName?.Trim(),
        CreatedAt = DateTime.UtcNow
    };

    public static CreateUserResponse MapToResponse(this User user /*Ulid ulid*/) => new(
        Ulid.New(user.Id),
        user.Email,
        user.Username,
        user.FirstName,
        user.LastName,
        user.DisplayName,
        user.CreatedAt);
}
