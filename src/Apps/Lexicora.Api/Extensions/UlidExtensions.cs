using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Lexicora.Api.Extensions;

public static class UlidExtensions
{
    public static bool TryParseRouteId(
        this string? id,
        out Ulid result,
        [NotNullWhen(false)] out ProblemHttpResult? error)
    {
        if (Ulid.TryParse(id, out result))
        {
            error = null;
            return true;
        }

        // Returns RFC 7807 compliant problem details
        error = TypedResults.Problem(
            type: "https://example.com/probs/invalid-id-format", // Custom type URI for documentation,
                                                                 // potential sites to put in type: "https://api.lexicora.com/errors/invalid-identifier",
                                                                 // "https://lexicora.api/errors/bad-request/invalid-ulid"
            detail: $"The value '{id}' is not a valid Ulid.",
            statusCode: StatusCodes.Status400BadRequest,
            title: "Invalid Identifier",
            extensions: new Dictionary<string, object?> { ["parameter"] = "id" }
        );
        return false;
    }
}
