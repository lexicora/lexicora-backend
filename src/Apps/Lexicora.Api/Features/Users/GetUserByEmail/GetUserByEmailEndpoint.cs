using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Entities;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetUserByEmail;

public class GetUserByEmailEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{email:email}", HandleAsync)
            .WithName("GetUserByEmail")
            .WithTags("Users")
            .WithSummary("Get user by email.")
            .WithDescription(
                "Gets a user by email. Navigation properties are not included, by default so no dto mapping necessary.");
    }

    private static async Task<Results<Ok<GetUserByEmailResponse>, NotFound>> HandleAsync(
        string email, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
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
                           WHERE email = @Email
                           """;

        var response = await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Email = email }
        );

        return response is not null
            ? TypedResults.Ok(response.MapToResponse())
            : TypedResults.NotFound();
    }
}
