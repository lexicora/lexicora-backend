using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Topics.CreateTopic;
using Lexicora.Api.Features.Topics.DeleteTopic;
using Lexicora.Api.Features.Topics.GetAllTopics;
using Lexicora.Api.Features.Topics.GetTopicById;
using Lexicora.Api.Features.Topics.GetTopicBySlug;
using Lexicora.Api.Features.Topics.GetTopicsByUserId;
using Lexicora.Api.Features.Topics.UpdateTopic;

namespace Lexicora.Api.Features.Topics;

public class TopicsConfiguration : IEndpointConfiguration
{
    public static void MapEndpoints(IEndpointRouteBuilder builder, bool? isDevelopment = null)
    {
        var topicsGroup = builder.MapGroup("/topics").WithTags("Topics");
        //.AddEndpointFilter<UlidIdValidationFilter>();

        // CREATE (POST)
        CreateTopicEndpoint.Map(topicsGroup);

        // READ (GET)
        GetTopicByIdEndpoint.Map(topicsGroup);
        GetTopicBySlugEndpoint.Map(topicsGroup);
        GetTopicsByUserIdEndpoint.Map(topicsGroup);

        // UPDATE (PUT/PATCH)
        UpdateTopicEndpoint.Map(topicsGroup);

        // DELETE
        DeleteTopicEndpoint.Map(topicsGroup);

        // Development Endpoints
        if (isDevelopment is true)
        {
            GetAllTopicsEndpoint.Map(topicsGroup);
        }
    }
}
