namespace Lexicora.Api.Contracts.Endpoints;

/// <summary>
/// Defines the configuration for mapping endpoints to an endpoint route builder.
/// </summary>
/// <remarks>
/// Implement this interface to configure and register endpoints within an application.
/// The <see cref="Map"/> method is called to apply the configuration to the provided <see cref="IEndpointRouteBuilder"/> instance.
/// Also, within the Map method implementation, a method called MapEndpoints can be used to map individual endpoints that implement the <see cref="IEndpoint"/> interface,
/// which belong to this endpoint configuration.
/// </remarks>
public interface IEndpointConfiguration
{
    /// <summary>
    /// Maps all specified endpoints within this method to the specified route builder.
    /// All endpoints that implement the <see cref="IEndpoint"/> interface and belong to this configuration should be registered here,
    /// or in the <see cref="MapEndpoints"/> method. (call it within this method implementation).
    /// <param name="endpointRouteBuilder"></param>
    /// </summary>
    static abstract void Map(IEndpointRouteBuilder endpointRouteBuilder);

    /// <summary>
    /// Maps individual endpoints that implement the <see cref="IEndpoint"/> interface, of which belong to this endpoint configuration.
    /// <param name="group">The group passed as parameter should be the route group of the EndpointConfiguration defined in the <see cref="Map"/> Method.</param>
    /// </summary>
    private static void MapEndpoints(IEndpointRouteBuilder group)
    {
        throw new NotImplementedException();
        // This method can be used to map individual endpoints that implement the IEndpoint interface.
        // It can be called within the Map method implementation of the IEndpointConfiguration.
    }
}
