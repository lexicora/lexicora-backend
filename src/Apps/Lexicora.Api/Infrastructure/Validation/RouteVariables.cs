using System.Text.RegularExpressions;

namespace Lexicora.Api.Infrastructure.Validation;

public static partial class RouteVariables
{
    [GeneratedRegex(@"^(?=.{6,254}$)[^@\s]+@[^@\s]+\.[^@\s]{2,}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex EmailRegex();

    [GeneratedRegex("^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._])){1,28}[a-zA-Z0-9]$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex UsernameRegex();

    [GeneratedRegex("^(?=.{14,100}$)[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$|^[a-zA-Z0-9]{26}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture)]
    private static partial Regex SlugRegex();

    public static bool IsValidEmail(string email) => EmailRegex().IsMatch(email);

    public static bool IsValidUsername(string username) => UsernameRegex().IsMatch(username);

    public static bool IsValidSlug(string slug) => SlugRegex().IsMatch(slug);
}
