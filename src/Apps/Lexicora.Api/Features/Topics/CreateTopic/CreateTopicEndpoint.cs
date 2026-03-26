using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Infrastructure.Filters;
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
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<CreateTopicRequest>>();
    }

    private static async Task<Results<Created<CreateTopicResponse>, Conflict<ProblemDetails>>> HandleAsync(
        [FromBody] CreateTopicRequest request,
        AppDbContext dbContext,
        CancellationToken cancellationToken)
    {

        var topic = request.MapToEntity();

        dbContext.Topics.Add(topic);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/topics/{topic.Id}", topic.MapToResponse());
    }
}
