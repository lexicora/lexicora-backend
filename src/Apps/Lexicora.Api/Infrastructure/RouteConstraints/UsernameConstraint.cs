using System.Text.RegularExpressions;

namespace Lexicora.Api.Infrastructure.RouteConstraints;

public partial class UsernameConstraint : IRouteConstraint
{
    [GeneratedRegex("^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._])){1,28}[a-zA-Z0-9]$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex UsernameRegex();

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string username)
        {
            // Simple regex for username validation. You can use a more complex one if needed.
            return UsernameRegex().IsMatch(username);
        }
        return false;
    }
}
