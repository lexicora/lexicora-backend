using Lexicora.Api.Infrastructure.Validation;
using Lexicora.Domain.Common;

namespace Lexicora.UnitTests.Domain;

public class CreateSlugTests
{
    private const string UlidString = "01M63Y9WESQWQXNVDDZKRXXBKV";

    [Fact]
    public void Builds_lowercase_slug_with_ulid_suffix()
    {
        var slug = Utilities.CreateSlug("Distributed Systems", UlidString);

        Assert.Equal("distributed-systems-nvddzkrxxbkv", slug);
    }

    [Theory]
    [InlineData("  Hello   World  ", "hello-world")]
    [InlineData("C# & .NET: Tips!", "c-net-tips")]
    [InlineData("snake_case--and  dashes", "snake-case-and-dashes")]
    [InlineData("-leading and trailing-", "leading-and-trailing")]
    public void Collapses_separators_and_drops_unsupported_characters(string title, string expectedPrefix)
    {
        var slug = Utilities.CreateSlug(title, UlidString);

        Assert.Equal($"{expectedPrefix}-nvddzkrxxbkv", slug);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("日本語のタイトル")]
    [InlineData("!!!")]
    public void Falls_back_to_full_ulid_when_title_has_no_usable_characters(string title)
    {
        var slug = Utilities.CreateSlug(title, UlidString);

        Assert.Equal(UlidString.ToLowerInvariant(), slug);
    }

    [Fact]
    public void Truncates_long_titles_to_the_maximum_slug_length()
    {
        var slug = Utilities.CreateSlug(new string('a', 500), UlidString);

        Assert.Equal(100, slug.Length);
        Assert.EndsWith("-nvddzkrxxbkv", slug);
    }

    [Theory]
    [InlineData("Distributed Systems")]
    [InlineData("a")]
    [InlineData("日本語")]
    [InlineData("word word word word word word word word word word word word word word word word word word word word")]
    public void Always_produces_a_slug_accepted_by_the_route_and_database_pattern(string title)
    {
        var slug = Utilities.CreateSlug(title, UlidString);

        Assert.True(RouteVariables.IsValidSlug(slug), $"'{slug}' does not match the slug pattern.");
    }
}
