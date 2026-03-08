using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Domain.Entities;
using Lexicora.Persistence.Postgres;

using Microsoft.AspNetCore.Http.HttpResults;

using Scalar.AspNetCore;

namespace Lexicora.Api.Features.Topics.GetTopicById;

public class GetTopicByIdEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id}", HandleAsync)
            //.WithGroupName("Topics")
            .WithName("GetTopicById")
            .WithDisplayName("GetTopicById")
            .WithTags("Topics", "Topic")
            .WithSummary("Gets a topic by id.")
            .WithDescription(
                "Gets a topic by id. Navigation properties are not included, by default so no dto mapping necessary.")
            .WithBadge("Topics", color: "blue");
        //.Produces(StatusCodes.Status200OK)
        //.ProducesProblem(StatusCodes.Status400BadRequest)
        //.ProducesProblem(StatusCodes.Status404NotFound);

    }

    private static async Task<Results<Ok<Topic>, NotFound, BadRequest<string>>> HandleAsync(string id, AppDbContext dbContext)
    {
        // Maybe extract this id conversion logic, to make it usable across the app, and to avoid code duplication.
        //var isValidId = Ulid.TryParse(id, CultureInfo.InvariantCulture, out var ulidId);
        var isValidId = Ulid.TryParse(id, out var ulidId);
        if (!isValidId) return TypedResults.BadRequest("Invalid id format.");

        var guidId = ulidId.ToGuid();

        var topic = await dbContext.Topics.FindAsync(guidId);

        return topic is not null
            ? TypedResults.Ok(topic /*TODO: Use Dto, for Guid to Ulid conversion*/)
            // Navigation properties are not included, by default so no dto mapping necessary.
            : TypedResults.NotFound();
    }
}
