using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Topics.CreateTopic;
using Lexicora.Api.Features.Topics.GetTopicById;
using Lexicora.Api.Features.Topics.GetTopicBySlug;
using Lexicora.Api.Features.Topics.GetTopicsByUserId;

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
        GetTopicBySlugEndpoint.Map(topicsGroup);
        GetTopicsByUserIdEndpoint.Map(topicsGroup);

        // CREATE (POST)
        CreateTopicEndpoint.Map(topicsGroup);

        // UPDATE (PUT/PATCH)

        // DELETE
    }
}
