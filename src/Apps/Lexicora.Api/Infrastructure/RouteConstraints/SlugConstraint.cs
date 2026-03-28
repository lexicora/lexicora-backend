using System.Text.RegularExpressions;

namespace Lexicora.Api.Infrastructure.RouteConstraints;

public partial class SlugConstraint : IRouteConstraint
{
    [GeneratedRegex("^(?=.{14,100}$)[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$|^[a-zA-Z0-9]{26}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex SlugRegex();

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string slug)
        {
            // Simple regex for slug validation. You can use a more complex one if needed.
            return SlugRegex().IsMatch(slug);
        }
        return false;
    }
}
