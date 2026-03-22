using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.GetUserById;

public static class GetUserByIdMappers
{
    public static GetUserByIdResponse MapToResponse(this User user) => new(
        Ulid.New(user.Id),
        user.Email,
        user.Username,
        user.FirstName,
        user.LastName,
        user.DisplayName,
        user.CreatedAt,
        user.UpdatedAt,
        user.EntryCount,
        user.TopicCount
    );
}
