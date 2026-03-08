namespace Lexicora.Api.Contracts.Endpoints;

/// <summary>
/// Defines the configuration for mapping endpoints to an endpoint route builder.
/// </summary>
/// <remarks>
/// Implement this interface to configure and register endpoints within an application.
/// The <see cref="MapEndpoints"/> method is called to apply the configuration to the provided <see cref="IEndpointRouteBuilder"/> instance.
/// Also, within the Map method implementation, a method called MapEndpoints can be used to map individual endpoints that implement the <see cref="IEndpoint"/> interface,
/// which belong to this endpoint configuration.
/// </remarks>
public interface IEndpointConfiguration
{
    /// <summary>
    /// Maps all specified endpoints within this method to the specified route builder.
    /// All endpoints that implement the <see cref="IEndpoint"/> interface and belong to this configuration should be registered here,
    /// or in a MapEndpoints method. (call it within this method implementation).
    /// </summary>
    /// <param name="builder">The root application or parent route group.</param>
    static abstract void MapEndpoints(IEndpointRouteBuilder builder);
}
