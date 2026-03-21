using System.Buffers;
using System.Runtime.CompilerServices;

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
        /// Creates a new instance of the <see cref="MonotonicGenerator"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="MonotonicGenerator"/> that can be used to generate monotonically increasing values.</returns>
        public static MonotonicGenerator CreateMonotonicGenerator() => new();

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
                // we let Cysharp handle the high-speed bit-shifting.
                return Ulid.TryParse(input, out result);
            }

            result = default;
            return false;
        }
        // Potentially create a PR on GitHub, so that this can be added to the Cysharp library as a built-in method.
    }
}

/// <summary>
/// Monotonic ULID generator. 
/// For generating ULID's that are guaranteed to be unique and ordered, even when generated in quick succession.
/// Extends the functionality of the standard ULID generator to ensure that each generated ULID is greater than the last one.
/// </summary>
/// <remarks>
/// This generator is useful in scenarios where you need to ensure that ULID's are generated in a strictly increasing order, such as in distributed systems or databases where uniqueness and order are critical.
/// Extends the standard <see cref="Ulid"/> implementation by Cysharp to provide a monotonic ULID generation mechanism.
/// </remarks>
public sealed class MonotonicGenerator
{
    private long _lastTimestamp = -1;
    private readonly byte[] _lastEntropy = new byte[10];
    private readonly Lock _lock = new();

    /// <summary>
    /// Generates a new ULID that is monotonic, meaning it will always be greater than the last generated ULID if called in quick succession.
    /// </summary>
    public Ulid NewUlid(DateTimeOffset? timestamp = null)
    {
        lock (_lock)
        {
            var timeToUse = timestamp ?? DateTimeOffset.UtcNow;
            var currentUnixTime = timeToUse.ToUnixTimeMilliseconds();

            if (currentUnixTime > _lastTimestamp)
            {
                // Generate new ULID with fresh entropy
                var ulidBytes = Ulid.NewUlid(timeToUse).ToByteArray();

                // Clear the Most Significant Bit (MSB) of the first entropy byte (index 6).
                // 0x7F is 01111111 in binary. This ensures we have 2^79 room for increments.
                ulidBytes[6] &= 0x7F;

                _lastTimestamp = currentUnixTime;
                Buffer.BlockCopy(ulidBytes, 6, _lastEntropy, 0, 10);

                return new Ulid(ulidBytes);
            }

            // Monotonic: increment entropy
            IncrementEntropy(_lastEntropy);

            var newUlidBytes = new byte[16];

            // Write the _lastTimestamp (48 bits, big endian)
            newUlidBytes[0] = ( byte )( _lastTimestamp >> 40 );
            newUlidBytes[1] = ( byte )( _lastTimestamp >> 32 );
            newUlidBytes[2] = ( byte )( _lastTimestamp >> 24 );
            newUlidBytes[3] = ( byte )( _lastTimestamp >> 16 );
            newUlidBytes[4] = ( byte )( _lastTimestamp >> 8 );
            newUlidBytes[5] = ( byte )_lastTimestamp;

            // Append entropy
            Buffer.BlockCopy(_lastEntropy, 0, newUlidBytes, 6, 10);

            return new Ulid(newUlidBytes);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void IncrementEntropy(byte[] entropy)
    {
        for (int i = entropy.Length - 1; i >= 0; i--)
        {
            if (++entropy[i] != 0) break;
        }
    }
}
