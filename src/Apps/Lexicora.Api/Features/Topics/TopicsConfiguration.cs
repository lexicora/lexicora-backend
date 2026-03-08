using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Topics.GetTopicById;

using Scalar.AspNetCore;

namespace Lexicora.Api.Features.Topics;

public class TopicsConfiguration : IEndpointConfiguration
{
    public static void Map(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var topicsGroup = endpointRouteBuilder.MapGroup("/topics")
            .WithGroupName("Topics Group")
            .WithName("Topics")
            .WithDisplayName("Topics")
            .WithTags("Topics", "Topic")
            .WithSummary("Topics endpoints.")
            .WithDescription("Topics endpoints.")
            .WithBadge("Topics", color: "blue");

        MapEndpoints(topicsGroup);
    }

    private static void MapEndpoints(IEndpointRouteBuilder group)
    {
        GetTopicByIdEndpoint.MapEndpoint(group);
    }
}
