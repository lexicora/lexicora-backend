using Lexicora.Api.Features.Topics;

namespace Lexicora.Api.Extensions;

public static class EndpointMapperExtensions
{
    /// <summary>
    /// Maps all configured endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    public static void MapConfiguredEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // Add the endpoints that implement IEndpointConfiguration here
        TopicsConfiguration.Map(endpointRouteBuilder);
    }
}
