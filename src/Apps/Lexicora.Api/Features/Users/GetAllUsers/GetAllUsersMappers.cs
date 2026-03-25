using Lexicora.Api.Features.Users.UpdateUser;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.GetAllUsers;

public static class GetAllUsersMappers
{
    public static UpdateUserResponse MapToResponse(this User user /*Ulid ulid*/) => new(
        Ulid.New(user.Id),
        user.Email,
        user.Username,
        user.FirstName,
        user.LastName,
        user.DisplayName,
        user.CreatedAt,
        user.UpdatedAt);
}
