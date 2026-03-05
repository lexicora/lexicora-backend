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
    public Ulid NewUlid()
    {
        lock (_lock)
        {
            var now = DateTimeOffset.UtcNow;
            var timestamp = now.ToUnixTimeMilliseconds();

            if (timestamp > _lastTimestamp)
            {
                // Generate new ULID with fresh entropy
                var ulid = Ulid.NewUlid(now);
                _lastTimestamp = timestamp;
                Buffer.BlockCopy(ulid.ToByteArray(), 6, _lastEntropy, 0, 10);
                return ulid;
            }

            // Monotonic: increment entropy
            IncrementEntropy(_lastEntropy);

            var ulidBytes = new byte[16];

            // Write timestamp (48 bits, big endian)
            ulidBytes[0] = ( byte )( timestamp >> 40 & 0xFF );
            ulidBytes[1] = ( byte )( timestamp >> 32 & 0xFF );
            ulidBytes[2] = ( byte )( timestamp >> 24 & 0xFF );
            ulidBytes[3] = ( byte )( timestamp >> 16 & 0xFF );
            ulidBytes[4] = ( byte )( timestamp >> 8 & 0xFF );
            ulidBytes[5] = ( byte )( timestamp & 0xFF );

            // Append entropy
            Buffer.BlockCopy(_lastEntropy, 0, ulidBytes, 6, 10);

            return new Ulid(ulidBytes);
        }
    }

    private static void IncrementEntropy(byte[] entropy)
    {
        for (int i = entropy.Length - 1; i >= 0; i--)
        {
            if (++entropy[i] != 0) break;
        }
    }
}
