using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

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
            .WithDescription("Gets all topics with an optional limit. The default limit is 1000. This endpoint is intended for testing and development purposes only and should not be exposed in production environments.")
            .WithParameterMetadata(0, "Maximum number of items to return. Must be non-negative, defaults to 1000.")
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<Ok<GetAllTopicsResponseCollection>, ProblemHttpResult>> HandleAsync(
        int? limit, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        if (limit < 0)
        {
            return TypedResults.Problem(
                title: "Invalid Limit",
                detail: $"Invalid limit value: '{limit}'. Limit must be a non-negative integer.",
                statusCode: StatusCodes.Status400BadRequest,
                extensions: new Dictionary<string, object?> { ["parameter"] = "limit" });
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
                     ORDER BY id -- ULIDs are time-ordered, so this sorts by creation time.
                     LIMIT @Limit
                     """;

        var topics = await connection.QueryAsync<GetAllTopicsResponse>(
            sql,
            new { Limit = limit ?? 1000 }
        );

        var response = new GetAllTopicsResponseCollection(topics.AsList());

        return TypedResults.Ok(response);
    }
}
