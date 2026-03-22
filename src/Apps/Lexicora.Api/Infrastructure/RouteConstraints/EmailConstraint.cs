using System.Text.RegularExpressions;

namespace Lexicora.Api.Infrastructure.RouteConstraints;

public partial class EmailConstraint : IRouteConstraint
{

    [GeneratedRegex(@"^(?=.{5,254}$)[^@\s]+@[^@\s]+\.[^@\s]{2,}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex EmailRegex();

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out var value) && value is string email)
        {
            // Simple regex for email validation. You can use a more complex one if needed.
            return EmailRegex().IsMatch(email);
        }
        return false;
    }

    //[GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
    //private static partial Regex EmailRegex();

    // Sample regex for "pro" validation not sure if it is 100% correct, since it's ai generated.
    //[GeneratedRegex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$",
    //    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
}
