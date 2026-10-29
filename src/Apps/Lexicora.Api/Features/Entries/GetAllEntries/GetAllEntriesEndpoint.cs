using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Entries.GetAllEntries;

public class GetAllEntriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/all", HandleAsync)
            .WithName("GetAllEntries")
            .WithTags("Development")
            .WithSummary("Get all entries")
            .WithDescription("Gets all entries (without their content blocks) with an optional limit. The default limit is 1000. This endpoint is intended for testing and development purposes only and should not be exposed in production environments.")
            .WithParameterMetadata(0, "Maximum number of items to return. Must be non-negative, defaults to 1000.")
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<Ok<GetAllEntriesResponseCollection>, ProblemHttpResult>> HandleAsync(
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
                           topic_id AS TopicId,
                           slug AS Slug,
                           title AS Title,
                           description AS Description,
                           tags AS Tags,
                           is_favorite AS IsFavorite,
                           favicon_url AS FaviconUrl,
                           url AS Url,
                           site_name AS SiteName,
                           excerpt AS Excerpt,
                           created_at AS CreatedAt,
                           updated_at AS UpdatedAt
                           FROM public.entries
                           ORDER BY id -- ULIDs are time-ordered, so this sorts by creation time.
                           LIMIT @Limit
                           """;

        var entries = await connection.QueryAsync<GetAllEntriesResponse>(
            sql,
            new { Limit = limit ?? 1000 }
        );

        var response = new GetAllEntriesResponseCollection(entries.AsList());

        return TypedResults.Ok(response);
    }
}
