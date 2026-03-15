using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Infrastructure.Filters;
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
            .WithSummary("Creates a new user.")
            .WithDescription(
                "Creates a new user. The request body must include the required properties for creating a user. Returns the created user on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<CreateUserRequest>>();
    }

    private static async Task<Results<Created<CreateUserResponse>, Conflict<ProblemDetails>>> HandleAsync(
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
            return TypedResults.Conflict(new ProblemDetails
            {
                Title = "User already exists",
                Detail = "A user with the given Email, or Username already exists.",
                Extensions =
                {
                    ["conflictingEmail"] = request.Email,
                    ["conflictingUsername"] = request.Username
                }
            });
        }

        var user = request.MapToEntity();

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = user.MapToResponse(); // maybe pass ulid as parameter
        //if (response.Id != new Ulid(user.Id)) throw new InvalidOperationException("The mapped response Id does not match the generated ULID.");

        return TypedResults.Created($"/users/{user.Id}", response);
    }
}
