using Lexicora.Api.Infrastructure.Validation;

namespace Lexicora.UnitTests.Api;

public class RouteVariablesTests
{
    [Theory]
    [InlineData("ada", true)]
    [InlineData("ada.lovelace", true)]
    [InlineData("ada_lovelace_1815", true)]
    [InlineData("ab", false)] // Too short
    [InlineData("a234567890123456789012345678901", false)] // Too long (31)
    [InlineData(".ada", false)] // Leading special character
    [InlineData("ada_", false)] // Trailing special character
    [InlineData("ada..lovelace", false)] // Consecutive special characters
    [InlineData("ada._lovelace", false)]
    [InlineData("ada-lovelace", false)] // Dash is not allowed
    public void Validates_usernames(string username, bool expected)
    {
        Assert.Equal(expected, RouteVariables.IsValidUsername(username));
    }

    [Theory]
    [InlineData("ada@example.com", true)]
    [InlineData("first.last+tag@sub.example.org", true)]
    [InlineData("not-an-email", false)]
    [InlineData("ada@example", false)]
    [InlineData("ada@example.c", false)] // TLD must have at least 2 characters
    [InlineData("a b@example.com", false)]
    public void Validates_emails(string email, bool expected)
    {
        Assert.Equal(expected, RouteVariables.IsValidEmail(email));
    }

    [Theory]
    [InlineData("distributed-systems-nvddzkrxxbkv", true)]
    [InlineData("01m63y9wesqwqxnvddzkrxxbkv", true)] // Bare ULID fallback
    [InlineData("distributed-systems", false)] // Missing 12 character suffix
    [InlineData("distributed--systems-nvddzkrxxbkv", false)] // Double dash
    public void Validates_slugs(string slug, bool expected)
    {
        Assert.Equal(expected, RouteVariables.IsValidSlug(slug));
    }
}
