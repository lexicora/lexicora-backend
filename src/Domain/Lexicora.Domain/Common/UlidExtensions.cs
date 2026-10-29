using System.Buffers;
using System.Text;

namespace Lexicora.Domain.Common;

public static class UlidExtensions
{
    // SearchValues is optimized by the JIT/AOT compiler for vectorized searching.
    // This set specifically excludes I, L, O, and U.
    private static readonly SearchValues<char> StrictCrockfordChars =
        SearchValues.Create("0123456789ABCDEFGHJKMNPQRSTVWXYZabcdefghjkmnpqrstvwxyz");

    extension(Ulid)
    {
        /// <summary>
        /// Validates and parses a ULID string using strict Crockford's Base32 rules.
        /// Rejects 'I', 'L', 'O', 'U' and enforces the 128-bit overflow limit.
        /// </summary>
        public static bool TryParseStrict(ReadOnlySpan<char> input, out Ulid result)
        {
            // Precise length check
            // Overflow check: The first character must be '0'-'7' 
            // (Because 26 chars at 5-bits each = 130 bits; ULID is only 128 bits)
            // Character validation: Rejecting aliased or invalid characters
            if (input.Length == 26 &&
                input[0] is >= '0' and <= '7' &&
                !input.ContainsAnyExcept(StrictCrockfordChars))
            {
                // Now that we've guaranteed the string is strictly valid,
                // We let default implementation handle the high-speed bit-shifting.
                result = Ulid.Parse(input);
                return true;
            }

            result = default;
            return false;
        }
    }

    extension(Ulid ulid)
    {
        /// <summary>
        /// Formats the ULID as a lowercase Crockford's Base32 string, matching the API's JSON representation.
        /// </summary>
        /// <remarks>Formats and lowercases directly into the new string's buffer, so only one allocation is made.</remarks>
        public string ToLowerString() =>
            string.Create(Ulid.UlidStringLength, ulid, static (buffer, value) =>
            {
                value.TryFormat(buffer, out _, [], null);
                Ascii.ToLowerInPlace(buffer, out _);
            });
    }
}
