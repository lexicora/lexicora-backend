using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Infrastructure.Filters;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Lexicora.Api.Features.Users.UpdateUser;

public class UpdateUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPut("/{id:ulid}", HandleAsync)
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithSummary("Update existing user")
            .WithDescription(
                "Updates an existing user. The request body must include the required properties for updating a user. Returns the updated user on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<UpdateUserRequest>>();
    }

    private static async Task<Results<Ok<UpdateUserResponse>, NotFound>> HandleAsync(
        string id,
        [FromBody] UpdateUserRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id);

        var existingUser = await dbContext.Users.FindAsync([ulid.ToGuid()], cancellationToken);

        if (existingUser is null) return TypedResults.NotFound();

        existingUser.UpdateWithRequestData(request);
        await dbContext.SaveChangesAsync(cancellationToken); // Save tracked changes to the database

        return TypedResults.Ok(existingUser.MapToResponse());
    }
}
