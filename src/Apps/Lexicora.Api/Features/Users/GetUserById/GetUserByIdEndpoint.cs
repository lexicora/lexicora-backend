using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetUserById;

public class GetUserByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:ulid}", HandleAsync)
            .WithName("GetUserById")
            .WithTags("Users")
            .WithSummary("Get user by id")
            .WithDescription("Gets a user by id.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<Ok<GetUserByIdResponse>, NotFound>> HandleAsync(
        string id, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

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
                           WHERE id = @Id
                           """;

        var user = await connection.QueryFirstOrDefaultAsync<GetUserByIdResponse>(
            sql,
            new { Id = ulid.ToGuid() }
        );

        return user is not null
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();
    }
}
