namespace Lexicora.Api.Extensions;

public static class EndpointMetadataExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder WithParameterMetadata(int indexOfParameter, string description)
        {
            // Add the metadata to the endpoint's metadata collection // lambda params: operation, context, ct
            return builder.AddOpenApiOperationTransformer((operation, _, _) =>
                {
                    var parameter = operation.Parameters?[indexOfParameter];
                    parameter?.Description = description;
                    return Task.CompletedTask;
                }
            );
        }

        public RouteHandlerBuilder WithUlidId(int indexOfParameter = 0, string description = "The unique Lexicographical Identifier (ULID).")
        {
            // This is a convenience method that combines the parameter metadata with the validation filter.
            return builder.WithParameterMetadata(indexOfParameter, description);
            //.AddEndpointFilter<UlidIdValidationFilter>();
        }
    }
}
