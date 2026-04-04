using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Lexicora.Api.Features.Users.DeleteUser;

public class DeleteUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("/{id:ulid}", HandleAsync)
            .WithName("DeleteUser")
            .WithTags("Users")
            .WithSummary("Delete User")
            .WithDescription("Deletes a user with given identifier.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        string id, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

        var userToDelete = await dbContext.Users.FindAsync([ulid.ToGuid()], cancellationToken);

        if (userToDelete is null) return TypedResults.NotFound();

        dbContext.Users.Remove(userToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
