using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.UpdateUser;

public static class UpdateUserMappers
{
    public static User MapToEntity(this UpdateUserRequest request /*Ulid ulid*/) => new()
    {
        Id = Ulid.New().ToGuid(),
        Email = request.Email.ToLowerInvariant(),
        Username = request.Username.ToLowerInvariant(),
        FirstName = request.FirstName.Trim(),
        LastName = request.LastName.Trim(),
        DisplayName = request.DisplayName?.Trim(),
        CreatedAt = DateTime.UtcNow
    };

    public static UpdateUserResponse MapToResponse(this User user /*Ulid ulid*/) => new(
        Ulid.New(user.Id),
        user.Email,
        user.Username,
        user.FirstName,
        user.LastName,
        user.DisplayName,
        user.CreatedAt);

    public static void UpdateUserWithRequest(this User user, UpdateUserRequest request)
    {
        user.Email = request.Email.ToLowerInvariant();
        user.Username = request.Username.ToLowerInvariant();
        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.DisplayName = request.DisplayName?.Trim();
        user.UpdatedAt = DateTime.UtcNow;
    }
}
