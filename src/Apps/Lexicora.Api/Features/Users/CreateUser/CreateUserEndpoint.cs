using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Common;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lexicora.Api.Features.Users.CreateUser;

public class CreateUserEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/", HandleAsync)
            .WithName("CreateUser")
            .WithTags("Users")
            .WithSummary("Create new user")
            .WithDescription(
                "Creates a new user. The request body must include the required properties for creating a user. Returns the created user on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static async Task<Results<CreatedAtRoute<CreateUserResponse>, ProblemHttpResult>> HandleAsync(
        [FromBody] CreateUserRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        // potentially check validity of request properties here.

        var exists = await dbContext.Users.AnyAsync(
            u => u.Email == request.Email || u.Username == request.Username,
            cancellationToken);

        if (exists)
        {
            return TypedResults.Problem(
                title: "User already exists",
                detail: "A user with the given Email, or Username already exists.",
                statusCode: StatusCodes.Status409Conflict,
                extensions: new Dictionary<string, object?>
                {
                    ["conflictingEmail"] = request.Email,
                    ["conflictingUsername"] = request.Username
                });
        }

        var user = request.MapToEntity();

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = user.MapToResponse();

        return TypedResults.CreatedAtRoute(
            response, "GetUserById", new RouteValueDictionary { ["id"] = response.Id.ToLowerString() });
    }
}
