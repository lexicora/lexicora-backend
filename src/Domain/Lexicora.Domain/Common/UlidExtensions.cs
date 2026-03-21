using System.Buffers;

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
                // we let default implementation handle the high-speed bit-shifting.
                return Ulid.TryParse(input, null, out result);
                //TODO: Maybe use regular parse, because validation is already done, and we want to avoid redundant checks in TryParse.
            }

            result = default;
            return false;
        }
    }
}
