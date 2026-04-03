using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Lexicora.Api.Features.Topics.DeleteTopic;

public class DeleteTopicEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapDelete("/{id:ulid}", HandleAsync)
            .WithName("DeleteTopic")
            .WithTags("Topics")
            .WithSummary("Delete Topic")
            .WithDescription("Deletes a topic with given identifier.")
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        string id, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var ulid = Ulid.Parse(id); // Safe, because of the route constraint.

        var topicToDelete = await dbContext.Topics.FindAsync([ulid.ToGuid()], cancellationToken);

        if (topicToDelete is null) return TypedResults.NotFound();

        dbContext.Topics.Remove(topicToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
