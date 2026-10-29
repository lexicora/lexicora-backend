using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Entries.GetEntriesByTopicId;

public class GetEntriesByTopicIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/by-topic/{topicId:ulid}", HandleAsync)
            .WithName("GetEntriesByTopicId")
            .WithTags("Entries")
            .WithSummary("Get entries by topic id")
            .WithDescription("Gets all entries of a topic, without their content blocks.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<Ok<List<GetEntriesByTopicIdResponse>>, NotFound>> HandleAsync(
        string topicId, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        var topicUlid = Ulid.Parse(topicId); // Safe, because of the route constraint.

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql = """
                           SELECT EXISTS (SELECT 1 FROM public.topics WHERE id = @TopicId);

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
                           WHERE topic_id = @TopicId
                           ORDER BY id; -- ULIDs are time-ordered, so this sorts by creation time.
                           """;

        var command = new CommandDefinition(
            sql,
            new { TopicId = topicUlid.ToGuid() },
            cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        var exists = await multi.ReadSingleAsync<bool>();

        if (!exists) return TypedResults.NotFound();

        var entries = await multi.ReadAsync<GetEntriesByTopicIdResponse>();

        return TypedResults.Ok(entries.AsList());
    }
}
