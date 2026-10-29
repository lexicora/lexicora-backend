using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Lexicora.Api.Features.Entries.DeleteEntry;

public class DeleteEntryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("/{id:ulid}", HandleAsync)
            .WithName("DeleteEntry")
            .WithTags("Entries")
            .WithSummary("Delete Entry")
            .WithDescription("Deletes an entry with given identifier, including its content blocks.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the entry.");
    }

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        string id, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

        var entryToDelete = await dbContext.Entries.FindAsync([ulid.ToGuid()], cancellationToken);

        if (entryToDelete is null) return TypedResults.NotFound();

        // Keep the denormalized counters in sync, all changes are saved in the same transaction.
        var topic = await dbContext.Topics.FindAsync([entryToDelete.TopicId], cancellationToken);
        if (topic is not null) topic.EntryCount--;

        var owner = await dbContext.Users.FindAsync([entryToDelete.UserId], cancellationToken);
        if (owner is not null) owner.EntryCount--;

        // Blocks are removed by the database's cascading delete.
        dbContext.Entries.Remove(entryToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
