using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Users.GetUserByEmail;

public static class GetUserByEmailMappers
{
    public static GetUserByEmailResponse MapToResponse(this User user) => new(
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
