using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;

using Microsoft.AspNetCore.Mvc;

namespace Lexicora.Api.Features.Topics.UpdateTopic;

public class UpdateTopicEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPut("/{id:ulid}", HandleAsync)
            .WithName("UpdateTopic")
            .WithTags("Topics")
            .WithSummary("Update Topic")
            .WithDescription("Updates a topic with given identifier.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<Ok<UpdateTopicResponse>, NotFound>> HandleAsync(
        string id,
        [FromBody] UpdateTopicRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id);

        var existingTopic = await dbContext.Topics.FindAsync([ulid.ToGuid()], cancellationToken);

        if (existingTopic is null) return TypedResults.NotFound();

        existingTopic.UpdateWithRequestData(request);
        await dbContext.SaveChangesAsync(cancellationToken); // Save tracked changes to the database

        return TypedResults.Ok(existingTopic.MapToResponse());
    }
}
