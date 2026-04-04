using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.CreateUser;

public static class CreateUserMappers
{
    public static User MapToEntity(this CreateUserRequest request /*Ulid ulid*/) => new(
        Ulid.New().ToGuid(),
        request.Email.ToLowerInvariant(),
        request.Username.ToLowerInvariant(),
        request.FirstName.Trim(),
        request.LastName.Trim(),
        request.DisplayName?.Trim()
    );

    public static CreateUserResponse MapToResponse(this User user /*Ulid ulid*/) => new(
        Ulid.New(user.Id),
        user.Email,
        user.Username,
        user.FirstName,
        user.LastName,
        user.DisplayName,
        user.CreatedAt);
}
