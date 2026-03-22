using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Entities;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetUserByUsername;

public class GetUserByUsernameEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{username:username}", HandleAsync)
            .WithName("GetUserByUsername")
            .WithTags("Users")
            .WithSummary("Get user by username.")
            .WithDescription("Gets a user by username.");
    }

    private static async Task<Results<Ok<GetUserByUsernameResponse>, NotFound>> HandleAsync(
        string username, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
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
                           WHERE username = @Username
                           """;

        var response = await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Username = username }
        );

        return response is not null
            ? TypedResults.Ok(response.MapToResponse())
            : TypedResults.NotFound();
    }
}
