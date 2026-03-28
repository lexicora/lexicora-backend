using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Api.Infrastructure.Validation;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetUserByUsername;

public class GetUserByUsernameEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/by-username/{username}", HandleAsync)
            .WithName("GetUserByUsername")
            .WithTags("Users")
            .WithSummary("Get user by username")
            .WithDescription("Gets a user by username.")
            .WithParameterMetadata(0, "The username of the user to retrieve. A valid username is required. <br>" +
                                      "Regex pattern: `^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._])){1,28}[a-zA-Z0-9]$`");
    }

    private static async Task<Results<Ok<GetUserByUsernameResponse>, NotFound, BadRequest<ProblemDetails>>> HandleAsync(
        string username, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        // Maybe implement validation only partially in regex for performance 
        if (!RouteVariables.IsValidUsername(username))
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid Username",
                Detail = $"The value '{username}' is not a valid Username.",
                Extensions =
                {
                    ["ValidUsernamePattern"] = "^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._])){1,28}[a-zA-Z0-9]$",
                    ["ValidUsernameDescription"] = "Valid Username must be between 3 and 30 characters and can contain letters, numbers, underscores and dots. It may not start with special characters."
                }
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
                           WHERE username = @Username::citext
                           """;

        var user = await connection.QueryFirstOrDefaultAsync<GetUserByUsernameResponse>(
            sql,
            new { Username = username }
        );

        return user is not null
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();
    }
}
