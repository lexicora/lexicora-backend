using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Extensions;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithUlidId(description: "The unique Lexicographical Identifier (ULID) for the topic.");
    }

    private static async Task<Results<Ok<GetTopicByIdResponse>, NotFound, ProblemHttpResult>> HandleAsync(string id, AppDbContext dbContext)
    {
        if (!id.TryParseRouteId(out var ulid, out var error))
            return error;

        var topic = await dbContext.Topics.AsNoTracking().FirstOrDefaultAsync(t => t.Id == ulid.ToGuid());

        return topic is not null
            ? TypedResults.Ok(topic.MapToResponse())
            : TypedResults.NotFound();
    }

    //private static async Task<Results<Ok<GetTopicByIdResponse>, NotFound>> HandleAsync(string id, AppDbContext dbContext)
    //{
    //    var ulid = Ulid.Parse(id); // This should never throw because of the validation filter.
    //    var topic = await dbContext.Topics.AsNoTracking().FirstOrDefaultAsync(t => t.Id == ulid.ToGuid());

    //    return topic is not null
    //        ? TypedResults.Ok(topic.MapToResponse())
    //        : TypedResults.NotFound();
    //}
}
