namespace Lexicora.Api.Infrastructure.Filters;

public class UlidIdValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // 1. Identify the 'id' parameter positionally. 
        // In Minimal APIs, the route {id} is usually the first argument.
        // We look for any string argument that matches the route key 'id'.
        var routeValues = context.HttpContext.Request.RouteValues;

        if (routeValues.TryGetValue("id", out var rawId) && rawId is string idString)
        {
            // 2. Perform the high-performance check.
            if (!Ulid.TryParse(idString, out _))
            {
                // 3. Return TypedResults for Source Gen compatibility.
                return TypedResults.Problem(
                    //type: "https://example.com/probs/invalid-id-format", // Custom type URI for documentation,
                    // potential sites to put in type: "https://api.lexicora.com/errors/invalid-identifier",
                    // "https://lexicora.api/errors/bad-request/invalid-ulid"
                    detail: $"The value '{idString}' is not a valid Ulid.",
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid Identifier Format",
                    extensions: new Dictionary<string, object?> { ["parameter"] = "id" }
                );
            }
        }

        return await next(context);
    }
}
