using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Lexicora.Api.Features.Entries.CreateEntry;

public class CreateEntryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/", HandleAsync)
            .WithName("CreateEntry")
            .WithTags("Entries")
            .WithSummary("Create new entry")
            .WithDescription("Creates a new entry, including its content blocks. Either an existing `topicId` or a new `topic` must be provided. Returns the created entry on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<CreatedAtRoute<CreateEntryResponse>, ProblemHttpResult>> HandleAsync(
        [FromBody] CreateEntryRequest request,
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        Topic? topic;
        User? user;

        if (!string.IsNullOrWhiteSpace(request.TopicId) && request.Topic is not null)
        {
            return InvalidRequest("Cannot provide both 'TopicId' and 'Topic'. Provide only one or the other.");
        }

        if (!string.IsNullOrWhiteSpace(request.TopicId))
        {
            var parsedTopicId = Ulid.Parse(request.TopicId).ToGuid(); // Safe, since it's validated by the request validation.
            topic = await dbContext.Topics.FindAsync([parsedTopicId], cancellationToken);

            if (topic is null)
            {
                return TypedResults.Problem(
                    title: "Invalid Topic",
                    detail: $"Topic with ID: '{request.TopicId}' does not exist.",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: new Dictionary<string, object?> { ["parameter"] = "topicId" });
            }

            // Can't be null, the foreign key guarantees the owner of the topic exists.
            user = await dbContext.Users.FindAsync([topic.UserId], cancellationToken);
        }
        else if (request.Topic is not null)
        {
            var parsedUserId = Ulid.Parse(request.Topic.UserId).ToGuid();
            user = await dbContext.Users.FindAsync([parsedUserId], cancellationToken);

            if (user is null)
            {
                return TypedResults.Problem(
                    title: "Invalid User",
                    detail: $"User with ID: '{request.Topic.UserId}' does not exist.",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: new Dictionary<string, object?> { ["parameter"] = "topic.userId" });
            }

            var topicId = Ulid.New();
            topic = new Topic(
                topicId.ToGuid(),
                parsedUserId,
                Utilities.CreateSlug(request.Topic.Name, topicId.ToString()),
                request.Topic.Name.Trim(),
                request.Topic.Description?.Trim(),
                [],
                request.Topic.IsFavorite
            );

            user.TopicCount++;
            dbContext.Topics.Add(topic);
        }
        else
        {
            return InvalidRequest("Either an existing TopicId or a new Topic must be provided.");
        }

        var entry = request.MapToEntity(topic.UserId, topic.Id);

        // Keep the denormalized counters in sync, everything is saved in the same transaction.
        topic.EntryCount++;
        user!.EntryCount++;

        dbContext.Entries.Add(entry);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = entry.MapToResponse();

        return TypedResults.CreatedAtRoute(
            response, "GetEntryById", new RouteValueDictionary { ["id"] = response.Id.ToLowerString() });
    }

    private static ProblemHttpResult InvalidRequest(string detail) => TypedResults.Problem(
        title: "Invalid Request",
        detail: detail,
        statusCode: StatusCodes.Status400BadRequest);
}
