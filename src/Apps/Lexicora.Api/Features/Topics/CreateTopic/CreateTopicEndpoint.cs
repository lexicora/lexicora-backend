using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Common;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Lexicora.Api.Features.Topics.CreateTopic;

public class CreateTopicEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/", HandleAsync)
            .WithName("CreateTopic")
            .WithTags("Topics")
            .WithSummary("Create new topic")
            .WithDescription("Creates a new topic. The request body must include the required properties for creating a topic. Returns the created topic on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<CreatedAtRoute<CreateTopicResponse>, ProblemHttpResult>> HandleAsync(
        [FromBody] CreateTopicRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userId = Ulid.Parse(request.UserId);

        var user = await dbContext.Users.FindAsync([userId.ToGuid()], cancellationToken);
        if (user is null)
        {
            return TypedResults.Problem(
                title: "Invalid User",
                detail: $"User with ID: '{request.UserId}' does not exist.",
                statusCode: StatusCodes.Status400BadRequest,
                extensions: new Dictionary<string, object?> { ["parameter"] = "userId" });
        }

        var topic = request.MapToEntity();

        user.TopicCount++;

        dbContext.Topics.Add(topic);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = topic.MapToResponse();

        return TypedResults.CreatedAtRoute(
            response, "GetTopicById", new RouteValueDictionary { ["id"] = response.Id.ToLowerString() });
    }
}
