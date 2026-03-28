using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Topics.GetTopicById;

public class GetTopicByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:ulid}", HandleAsync)
            .WithName("GetTopicById")
            .WithTags("Topics")
            .WithSummary("Get topic by id")
            .WithDescription(
                "Gets a topic by id. Navigation properties are not included, by default so no dto mapping necessary.") // Supposedly supports markdown and used for extensive documentation.
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<Ok<GetTopicByIdResponse>, NotFound>> HandleAsync(
        string id, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

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
                           WHERE id = @Id
                           """;

        var topic = await connection.QueryFirstOrDefaultAsync<GetTopicByIdResponse>(
            sql,
            new { Id = ulid.ToGuid() } // Pass the parsed GUID as a safe SQL parameter
        );

        return topic is not null
            ? TypedResults.Ok(topic)
            : TypedResults.NotFound();
    }
}
