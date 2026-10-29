using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Api.Features.Entries._Shared;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lexicora.Api.Features.Entries.UpdateEntry;

public class UpdateEntryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPut("/{id:ulid}", HandleAsync)
            .WithName("UpdateEntry")
            .WithTags("Entries")
            .WithSummary("Update Entry")
            .WithDescription("Updates an entry with given identifier. If `blocks` are provided, they replace the entry's content entirely, otherwise the content stays unchanged.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the entry.")
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<Ok<UpdateEntryResponse>, NotFound>> HandleAsync(
        string id,
        [FromBody] UpdateEntryRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

        var existingEntry = await dbContext.Entries.FindAsync([ulid.ToGuid()], cancellationToken);

        if (existingEntry is null) return TypedResults.NotFound();

        // The bulk delete runs immediately, so an explicit transaction keeps the content replacement atomic.
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        if (request.Blocks is not null)
        {
            // A single DELETE statement, child blocks are covered by it too, without loading anything.
            await dbContext.Blocks
                .Where(b => b.EntryId == existingEntry.Id)
                .ExecuteDeleteAsync(cancellationToken);

            dbContext.Blocks.AddRange(request.Blocks.MapToEntities(existingEntry.Id));
        }

        existingEntry.UpdateWithRequestData(request);
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return TypedResults.Ok(existingEntry.MapToResponse());
    }
}
