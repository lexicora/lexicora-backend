using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Entities;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetAllUsers;

public class GetAllUsersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/", HandleAsync)
            .WithName("GetAllUsers")
            .WithTags("Users")
            .WithSummary("Get all users")
            .WithDescription("Gets all users. This endpoint is intended for testing and development purposes only and should not be exposed in production environments.");
    }

    private static async Task<Ok<List<GetAllUsersResponse>>> HandleAsync(
        NpgsqlDataSource dataSource, CancellationToken cancellationToken)
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
                     """;

        var users = await connection.QueryAsync<User>(
            sql
        );

        var userList = new List<GetAllUsersResponse>();
        return TypedResults.Ok(userList);
    }
}
