using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.UpdateUser;

public static class UpdateUserMappers
{
    extension(User user)
    {
        public UpdateUserResponse MapToResponse() => new(
            Ulid.New(user.Id),
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            user.DisplayName,
            user.CreatedAt,
            user.UpdatedAt);

        public void UpdateWithRequestData(UpdateUserRequest request)
        {
            user.Email = request.Email.ToLowerInvariant();
            user.Username = request.Username.ToLowerInvariant();
            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.DisplayName = request.DisplayName?.Trim();
            user.UpdatedAt = DateTime.UtcNow;
        }
    }
}
