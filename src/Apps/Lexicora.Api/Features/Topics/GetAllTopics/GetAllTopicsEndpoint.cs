using Dapper;

using Lexicora.Api.Contracts.Endpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace Lexicora.Api.Features.Topics.GetAllTopics;

public class GetAllTopicsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/all", HandleAsync)
            .WithName("GetAllTopics")
            .WithTags("Development")
            .WithSummary("Get all topics")
            .WithDescription("Gets all topics with an optional limit. The default limit is 1000. This endpoint is intended for testing and development purposes only and should not be exposed in production environments.");
    }

    private static async Task<Results<Ok<GetAllTopicsResponseCollection>, BadRequest<ProblemDetails>>> HandleAsync(
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
                     user_id AS UserId,
                     slug AS Slug,
                     name AS Name, 
                     description AS Description,
                     is_favorite AS IsFavorite,
                     tags AS Tags,
                     created_at AS CreatedAt, 
                     updated_at AS UpdatedAt,
                     entry_count AS EntryCount
                     FROM public.topics
                     LIMIT @Limit
                     """;

        var users = await connection.QueryAsync<GetAllTopicsResponse>(
            sql,
            new { Limit = limit ?? 1000 }
        );

        var userList = users.AsList();

        var response = new GetAllTopicsResponseCollection(userList);

        return TypedResults.Ok(response);
    }
}
