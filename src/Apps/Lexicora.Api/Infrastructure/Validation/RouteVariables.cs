using System.Text.RegularExpressions;

namespace Lexicora.Api.Infrastructure.Validation;

public static partial class RouteVariables
{
    [GeneratedRegex(@"^(?=.{5,254}$)[^@\s]+@[^@\s]+\.[^@\s]{2,}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex EmailRegex();

    [GeneratedRegex("^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._])){1,28}[a-zA-Z0-9]$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex UsernameRegex();


    public static bool IsValidEmail(string email) => EmailRegex().IsMatch(email);

    public static bool IsValidUsername(string username) => UsernameRegex().IsMatch(username);
}
