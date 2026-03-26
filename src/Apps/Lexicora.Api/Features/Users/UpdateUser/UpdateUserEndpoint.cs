using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Infrastructure.Filters;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    private static async Task<Results<Ok<UpdateUserResponse>, NotFound, Conflict<ProblemDetails>>> HandleAsync(
        string id,
        [FromBody] UpdateUserRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id);
        var guidId = ulid.ToGuid();

        var conflictExists = await dbContext.Users.AnyAsync(
            u => u.Id != guidId && ( u.Email == request.Email || u.Username == request.Username ),
            cancellationToken);

        if (conflictExists)
        {
            return TypedResults.Conflict(new ProblemDetails
            {
                Title = "Email and or Username not available",
                Detail = "Another user with the given Email and or Username already exists.",
                Extensions =
                {
                    ["conflictingEmail"] = request.Email,
                    ["conflictingUsername"] = request.Username
                }
            });
        }

        var existingUser = await dbContext.Users.FindAsync([guidId], cancellationToken);

        if (existingUser is null) return TypedResults.NotFound();

        existingUser.UpdateWithRequestData(request);
        await dbContext.SaveChangesAsync(cancellationToken); // Save tracked changes to the database

        return TypedResults.Ok(existingUser.MapToResponse());
    }
}
