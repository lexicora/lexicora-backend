namespace Lexicora.Api.Contracts.Endpoints;

/// <summary>
///    Interface for defining Minimal API endpoints.
/// </summary>
/// <remarks>
/// Implement this interface to create and register an endpoints within an <see cref="IEndpointConfiguration"/>.
/// The <see cref="MapEndpoint"/> method is called to apply the endpoint configuration to the provided <see cref="IEndpointRouteBuilder"/> instance.
/// 
/// It's recommended that implementing classes define a `HandleAsync` or `Handle` method 
/// to be passed into routing functions like `MapGet`, `MapPost`, etc.
/// This method should serve as the logic for handling requests / responses of the endpoint.
/// </remarks>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified route builder.
    /// <param name="group"></param>
    /// </summary>
    static abstract void MapEndpoint(IEndpointRouteBuilder group); // TODO: Maybe rename it to just Map.
}
