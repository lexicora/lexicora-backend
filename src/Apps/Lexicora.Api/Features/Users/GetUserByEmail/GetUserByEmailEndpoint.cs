using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Api.Infrastructure.Validation;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetUserByEmail;

public class GetUserByEmailEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/by-email/{email}", HandleAsync)
            .WithName("GetUserByEmail")
            .WithTags("Users")
            .WithSummary("Get user by email")
            .WithDescription("Gets a user by email.")
            .WithParameterMetadata(0, "The email of the user to retrieve. A valid email address is required.");
    }

    private static async Task<Results<Ok<GetUserByEmailResponse>, NotFound, BadRequest<ProblemDetails>>> HandleAsync(
        string email, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        // Maybe implement validation only partially in regex for performance
        if (!RouteVariables.IsValidEmail(email))
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid Email",
                Detail = $"The value '{email}' is not a valid email address.",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { ["parameter"] = "email" }
            });
        }

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql = """
                           SELECT 
                           id AS "Id",
                           email AS "Email",
                           username AS "Username",
                           first_name AS "FirstName",
                           last_name AS "LastName",
                           display_name AS "DisplayName",
                           created_at AS "CreatedAt",
                           updated_at AS "UpdatedAt",
                           entry_count AS "EntryCount",
                           topic_count AS "TopicCount"
                           FROM "public"."users"
                           WHERE email = @Email
                           """;

        var user = await connection.QueryFirstOrDefaultAsync<GetUserByEmailResponse>(
            sql,
            new { Email = email }
        );

        return user is not null
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();
    }
}
