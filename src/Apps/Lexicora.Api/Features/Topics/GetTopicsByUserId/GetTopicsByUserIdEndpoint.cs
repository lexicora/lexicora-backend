using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Topics.GetTopicsByUserId;

public class GetTopicsByUserIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/by-user/{userId:ulid}", HandleAsync)
            .WithName("GetTopicsByUserId")
            .WithTags("Topics")
            .WithSummary("Get topics by user id")
            .WithDescription(
                "Gets topics by user id. Navigation properties are not included, by default so no dto mapping necessary.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the user.");
    }

    private static async Task<Results<Ok<List<GetTopicsByUserIdResponse>>, NotFound>> HandleAsync(
        string userId, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        /*
         * TODO: Later add search parameters for filtering, sorting, and pagination.
         * Also add (string? filter...) and other properties for extensive search and virtualization for this endpoint
         * For example, filter by name or tags, sort by created date or name, and paginate with limit and offset or cursor-based pagination.
         */

        var userUlid = Ulid.Parse(userId); // Safe, because of the route constraint.

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql = """
                           SELECT EXISTS (SELECT 1 FROM public.users WHERE id = @UserId);

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
                           WHERE user_id = @UserId
                           ORDER BY id; -- ULIDs are time-ordered, so this sorts by creation time.

                           """;

        var command = new CommandDefinition(
            sql,
            new { UserId = userUlid.ToGuid() },
            cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        var exists = await multi.ReadSingleAsync<bool>();

        if (!exists) return TypedResults.NotFound();

        var topics = await multi.ReadAsync<GetTopicsByUserIdResponse>();

        return TypedResults.Ok(topics.AsList());
    }
}
