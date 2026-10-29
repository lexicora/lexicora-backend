using Lexicora.Domain.Common;

namespace Lexicora.UnitTests.Domain;

public class UlidExtensionsTests
{
    [Theory]
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAV")]
    [InlineData("01arz3ndektsv4rrffq69g5fav")]
    [InlineData("7ZZZZZZZZZZZZZZZZZZZZZZZZZ")] // Largest value that still fits into 128 bits
    public void TryParseStrict_accepts_valid_ulids(string input)
    {
        Assert.True(Ulid.TryParseStrict(input, out var result));
        Assert.Equal(Ulid.Parse(input), result);
    }

    [Theory]
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FA")] // Too short
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAVX")] // Too long
    [InlineData("8ZZZZZZZZZZZZZZZZZZZZZZZZZ")] // Overflows 128 bits
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAI")] // 'I' is an alias, not a canonical character
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAL")] // 'L' is an alias
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAO")] // 'O' is an alias
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAU")] // 'U' is excluded from Crockford's Base32
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FA-")]
    [InlineData("")]
    public void TryParseStrict_rejects_invalid_input(string input)
    {
        Assert.False(Ulid.TryParseStrict(input, out var result));
        Assert.Equal(default, result);
    }

    [Fact]
    public void ToLowerString_matches_lowercased_canonical_representation()
    {
        var ulid = Ulid.New();

        Assert.Equal(ulid.ToString().ToLowerInvariant(), ulid.ToLowerString());
    }

    [Fact]
    public void Guid_conversion_round_trips()
    {
        // ULIDs are stored as Postgres uuid columns, so the conversion must be lossless.
        var ulid = Ulid.New();

        Assert.Equal(ulid, Ulid.New(ulid.ToGuid()));
    }
}
