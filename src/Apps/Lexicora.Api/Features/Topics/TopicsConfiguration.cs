using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Topics.GetTopicById;

using Scalar.AspNetCore;

namespace Lexicora.Api.Features.Topics;

public class TopicsConfiguration : IEndpointConfiguration
{
    public static void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var topicsGroup = builder.MapGroup("/topics")
            .WithTags("Topics")
            .WithDescription("Endpoints for managing and retrieving topic data.")
            .WithBadge("Topics", color: "blue");
        //.AddEndpointFilter<UlidIdValidationFilter>();

        // GET
        GetTopicByIdEndpoint.Map(topicsGroup);

        // POST

        // UPDATE (PUT/PATCH)

        // DELETE
    }
}
