using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;

using Scalar.AspNetCore;

namespace Lexicora.Api.Features.Topics.GetTopicById;

public class GetTopicByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id}", HandleAsync)
            .WithName("GetTopicById")
            .WithTags("Topics")
            .WithSummary("Gets a topic by id.")
            .WithDescription(
                "Gets a topic by id. Navigation properties are not included, by default so no dto mapping necessary.") // Supposedly supports markdown and used for extensive documentation.
            .WithBadge("Topics", color: "blue") // Optional for scalar
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Results<Ok<GetTopicByIdResponse>, NotFound>> HandleAsync(Ulid id, AppDbContext dbContext)
    {
        var topic = await dbContext.Topics.FindAsync(id.ToGuid());

        return topic is not null
            ? TypedResults.Ok(topic.MapToResponse())
            : TypedResults.NotFound();
    }
}
