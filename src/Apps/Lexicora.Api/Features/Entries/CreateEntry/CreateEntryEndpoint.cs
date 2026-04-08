using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Infrastructure.Filters;
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
            .WithDescription("Creates a new entry. The request body must include the required properties for creating an entry. Returns the created entry on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<CreateEntryRequest>>();
    }

    private static async Task<Results<Created<CreateEntryResponse>, BadRequest<ProblemDetails>>> HandleAsync(
        [FromBody] CreateEntryRequest request,
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        Topic? topic;
        User? user;
        Guid userId;

        if (!string.IsNullOrWhiteSpace(request.TopicId) && request.Topic is not null)
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Cannot provide both 'TopicId' and 'Topic'. Provide only one or the other.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        if (!string.IsNullOrWhiteSpace(request.TopicId))
        {
            var parsedTopicId = Ulid.Parse(request.TopicId).ToGuid();
            topic = await dbContext.Topics.FindAsync([parsedTopicId], cancellationToken);

            if (topic is null)
            {
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Invalid Topic",
                    Detail = $"Topic with ID: '{request.TopicId}' does not exist.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            userId = topic.UserId;

            user = await dbContext.Users.FindAsync([userId], cancellationToken);
            if (user is null)
            {
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Invalid User",
                    Detail = "User associated with the topic does not exist.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
        else if (request.Topic is not null)
        {
            var parsedUserId = Ulid.Parse(request.Topic.UserId).ToGuid();
            user = await dbContext.Users.FindAsync([parsedUserId], cancellationToken);

            if (user is null)
            {
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Invalid User",
                    Detail = $"User with ID: '{request.Topic.UserId}' does not exist.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var topicId = Ulid.New();
            topic = new Topic(
                topicId.ToGuid(),
                parsedUserId,
                Utilities.CreateSlug(request.Topic.Name, topicId.ToString()),
                request.Topic.Name.Trim(),
                request.Topic.Description?.Trim(),
                []
            );

            user.TopicCount++;
            dbContext.Topics.Add(topic);

            userId = parsedUserId;
        }
        else
        {
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Either an existing TopicId or a new Topic must be provided.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var entry = request.MapToEntity(userId, topic.Id);

        topic.EntryCount++;
        user.EntryCount++;

        dbContext.Entries.Add(entry);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/entries/{entry.Id}", entry.MapToResponse());
    }
}
