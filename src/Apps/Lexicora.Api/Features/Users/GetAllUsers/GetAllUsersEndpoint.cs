using Dapper;

using Lexicora.Api.Contracts.Endpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace Lexicora.Api.Features.Users.GetAllUsers;

public class GetAllUsersEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/all", HandleAsync)
            .WithName("GetAllUsers")
            .WithTags("Users", "Development")
            .WithSummary("Get all users")
            .WithDescription("Gets all users with an optional limit. The default limit is 1000. This endpoint is intended for testing and development purposes only and should not be exposed in production environments.");
    }

    private static async Task<Results<Ok<GetAllUsersResponseCollection>, BadRequest<ProblemDetails>>> HandleAsync(
        int? limit, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        if (limit < 0)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid Limit",
                Detail = $"Invalid limit value: '{limit}'. Limit must be a non-negative integer.",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql = """
                     SELECT 
                     id AS Id,
                     email AS Email,
                     username AS Username,
                     first_name AS FirstName,
                     last_name AS LastName,
                     display_name AS DisplayName,
                     created_at AS CreatedAt,
                     updated_at AS UpdatedAt,
                     entry_count AS EntryCount,
                     topic_count AS TopicCount
                     FROM public.users
                     LIMIT @Limit
                     """;

        var users = await connection.QueryAsync<GetAllUsersResponse>(
            sql,
            new { Limit = limit ?? 1000 }
        );

        var userList = users.AsList();

        var response = new GetAllUsersResponseCollection(
            TotalCount: userList.Count,
            Users: userList
        );

        return TypedResults.Ok(response);
    }
    /*
     * TODO: Consider tailored endpoints for specific cases to be used in production.
     * Examples: Look at GitHub issue: https://github.com/lexicora/lexicora-backend/issues/19
     */
}
