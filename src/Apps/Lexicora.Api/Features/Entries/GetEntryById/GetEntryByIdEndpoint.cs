using Dapper;

using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;

using Microsoft.AspNetCore.Http.HttpResults;

using Npgsql;

namespace Lexicora.Api.Features.Entries.GetEntryById;

public class GetEntryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:ulid}", HandleAsync)
            .WithName("GetEntryById")
            .WithTags("Entries")
            .WithSummary("Get entry by id")
            .WithDescription("Gets an entry by id, including its content as a nested BlockNote document.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the entry.");
    }

    private static async Task<Results<Ok<GetEntryByIdResponse>, NotFound>> HandleAsync(
        string id, NpgsqlDataSource dataSource, CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        // One round trip for the entry and all of its blocks. The jsonb columns are read as text
        // and written to the response verbatim, so the block content is never parsed.
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
                           origin_url AS OriginUrl,
                           path_url AS PathUrl,
                           language_code AS LanguageCode,
                           created_at AS CreatedAt,
                           updated_at AS UpdatedAt,
                           site_name AS SiteName,
                           excerpt AS Excerpt,
                           byline AS Byline,
                           published_at AS PublishedAt
                           FROM public.entries
                           WHERE id = @Id;

                           SELECT
                           id AS Id,
                           parent_block_id AS ParentBlockId,
                           type AS Type,
                           props_json::text AS PropsJson,
                           content_json::text AS ContentJson
                           FROM public.blocks
                           WHERE entry_id = @Id
                           ORDER BY "order";
                           """;

        var command = new CommandDefinition(sql, new { Id = ulid.ToGuid() }, cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        var entry = await multi.ReadSingleOrDefaultAsync<GetEntryByIdResponse>();
        if (entry is null) return TypedResults.NotFound();

        var blocks = (await multi.ReadAsync<BlockResponse>()).AsList();
        entry.Blocks = BuildBlockTree(blocks);

        return TypedResults.Ok(entry);
    }

    /// <summary>
    /// Rebuilds the nested document from the flat block rows in a single pass.
    /// </summary>
    /// <remarks>The rows are sorted by order, so every list of children ends up in document order as well.</remarks>
    private static List<BlockResponse> BuildBlockTree(List<BlockResponse> blocks)
    {
        var blocksById = new Dictionary<Guid, BlockResponse>(blocks.Count);
        foreach (var block in blocks)
        {
            blocksById[block.Id] = block;
        }

        var rootBlocks = new List<BlockResponse>();
        foreach (var block in blocks)
        {
            if (block.ParentBlockId is { } parentId && blocksById.TryGetValue(parentId, out var parent))
            {
                parent.Children.Add(block);
            }
            else
            {
                rootBlocks.Add(block);
            }
        }

        return rootBlocks;
    }
}
