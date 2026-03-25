using Lexicora.Api.Features.Topics;
using Lexicora.Api.Features.Users;

namespace Lexicora.Api.Extensions;

public static class EndpointMapperExtensions
{
    /// <summary>
    /// Maps all configured endpoints to the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The application to which all endpoints will be mapped.</param>
    public static void MapConfiguredEndpoints(this WebApplication app)
    {
        var isDevelopment = app.Environment.IsDevelopment();

        // If needed, add versioning or a global route prefix here before mapping the endpoints.

        // Add the endpoints that implement IEndpointConfiguration here
        TopicsConfiguration.MapEndpoints(app, isDevelopment);
        UsersConfiguration.MapEndpoints(app, isDevelopment);
    }
}
