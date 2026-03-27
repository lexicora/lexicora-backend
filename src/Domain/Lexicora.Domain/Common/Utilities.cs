namespace Lexicora.Domain.Common;

public static class Utilities
{
    public static string CreateSlug(string title, string ulidString)
    {
        if (string.IsNullOrWhiteSpace(title)) return ulidString.ToLowerInvariant();

        const int maxTitleLength = 87;
        const int suffixLength = 12;

        // Safely extract suffix length without throwing an exception.
        // Tip: Using the end of a ULID gives better uniqueness (randomness) than the start (timestamp).
        ReadOnlySpan<char> ulidSpan = ulidString.AsSpan();
        if (ulidSpan.Length > suffixLength)
        {
            ulidSpan = ulidSpan[^suffixLength..]; // take the last 12 chars
        }

        // Total max length: 87 (title) + 1 (separator dash) + 12 (suffix) = 100 chars
        Span<char> buffer = stackalloc char[maxTitleLength + 1 + suffixLength];
        int pos = 0;

        ReadOnlySpan<char> input = title.AsSpan().Trim();

        foreach (char c in input)
        {
            if (pos >= maxTitleLength) break;

            if (char.IsLetterOrDigit(c))
            {
                buffer[pos++] = char.ToLowerInvariant(c);
            }
            else if (c is ' ' or '-' or '_')
            {
                // Prevent leading and double dashes
                if (pos > 0 && buffer[pos - 1] != '-')
                {
                    buffer[pos++] = '-';
                }
            }
        }

        // Trim trailing dash if the title ended in a special char or was truncated perfectly at a dash
        if (pos > 0 && buffer[pos - 1] == '-')
        {
            pos--;
        }

        // Add the separator between title and suffix if we actually parsed title characters
        if (pos > 0)
        {
            buffer[pos++] = '-';
        }

        // Append the ULID suffix
        for (int i = 0; i < ulidSpan.Length; i++)
        {
            buffer[pos++] = char.ToLowerInvariant(ulidSpan[i]);
        }

        // Only one string allocation here!
        return new string(buffer[..pos]);
    }
}
