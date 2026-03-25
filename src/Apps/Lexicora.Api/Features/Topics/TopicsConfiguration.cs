using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Topics.GetTopicById;

namespace Lexicora.Api.Features.Topics;

public class TopicsConfiguration : IEndpointConfiguration
{
    public static void MapEndpoints(IEndpointRouteBuilder builder, bool? isDevelopment = null)
    {
        var topicsGroup = builder.MapGroup("/topics")
            .WithTags("Topics");
        //.AddEndpointFilter<UlidIdValidationFilter>();

        // READ (GET)
        GetTopicByIdEndpoint.Map(topicsGroup);

        // CREATE (POST)

        // UPDATE (PUT/PATCH)

        // DELETE
    }
}
