using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Topics.GetTopicBySlug;

public class GetTopicBySlugEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/by-slug/{slug:slug}", HandleAsync)
            .WithName("GetTopicBySlug")
            .WithTags("Topics")
            .WithSummary("Get topic by slug")
            .WithDescription(
                "Gets a topic by slug. Navigation properties are not included, by default so no dto mapping necessary.") // Supposedly supports markdown and used for extensive documentation.
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<Ok<GetTopicBySlugResponse>, NotFound>> HandleAsync(
        string slug, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
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
                           WHERE slug = @Slug::citext
                           """;

        var topic = await connection.QueryFirstOrDefaultAsync<GetTopicBySlugResponse>(
            sql,
            new { Slug = slug } // Pass the slug as a safe SQL parameter
        );

        return topic is not null
            ? TypedResults.Ok(topic)
            : TypedResults.NotFound();
    }
}
