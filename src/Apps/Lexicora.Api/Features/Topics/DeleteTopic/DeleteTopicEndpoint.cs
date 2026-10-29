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

        // Keep the denormalized counter in sync, both changes are saved in the same transaction.
        var owner = await dbContext.Users.FindAsync([topicToDelete.UserId], cancellationToken);
        if (owner is not null)
        {
            owner.TopicCount--;
            owner.EntryCount -= topicToDelete.EntryCount; // The topic's entries are removed by the cascading delete.
        }

        dbContext.Topics.Remove(topicToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
