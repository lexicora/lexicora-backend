using Lexicora.Domain.Common;

namespace Lexicora.Api.Infrastructure.RouteConstraints;

public class UlidConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string stringValue)
        {
            // ByteAether's Ulid.TryParseStrict is very efficient (0 allocations)
            return Ulid.TryParseStrict(stringValue, out _);
        }
        return false;
    }
}
