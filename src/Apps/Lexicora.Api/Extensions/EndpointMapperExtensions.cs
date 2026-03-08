using Lexicora.Api.Features.Topics;

namespace Lexicora.Api.Extensions;

public static class EndpointMapperExtensions
{
    /// <summary>
    /// Maps all configured endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="baseAppRoute">The root application route builder to which all endpoints will be mapped.</param>
    public static void MapConfiguredEndpoints(this IEndpointRouteBuilder baseAppRoute)
    {
        // Add the endpoints that implement IEndpointConfiguration here
        TopicsConfiguration.MapEndpoints(baseAppRoute);
    }
}
