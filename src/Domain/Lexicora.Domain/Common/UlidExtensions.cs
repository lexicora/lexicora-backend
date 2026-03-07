using System.Runtime.CompilerServices;

namespace Lexicora.Domain.Common;

public static class UlidExtensions
{
    extension(Ulid)
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MonotonicGenerator"/> class.
        /// </summary>
        /// <returns>A new instance of <see cref="MonotonicGenerator"/> that can be used to generate monotonically increasing values.</returns>
        public static MonotonicGenerator CreateMonotonicGenerator() => new();
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
                // 1. Generate new ULID with fresh entropy
                var ulidBytes = Ulid.NewUlid(timeToUse).ToByteArray();

                // 2. Clear the Most Significant Bit (MSB) of the first entropy byte (index 6).
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
