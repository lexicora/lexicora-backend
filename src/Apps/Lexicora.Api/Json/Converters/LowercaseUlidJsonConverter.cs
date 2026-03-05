using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lexicora.Api.Json.Converters;

/// <summary>
/// JsonConverter for ULID that serializes and deserializes ULIDs as lowercase strings.
/// </summary>
/// <remarks>
/// Similar to the original implementation by Cysharp, but ensures that the output is always in lowercase.
/// This might be beneficial for displaying ULIDs in a URL (publicly).
/// </remarks>
public class LowercaseUlidJsonConverter : JsonConverter<Ulid>
{
    /// <summary>
    /// Read a Ulid value represented by a string from JSON.
    /// </summary>
    /// <remarks>
    /// Same as original implementation by Cysharp.
    /// </remarks>
    public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            if (reader.TokenType != JsonTokenType.String) throw new JsonException("Expected string");

            if (reader.HasValueSequence)
            {
                // Parse using ValueSequence
                var seq = reader.ValueSequence;
                if (seq.Length != 26) throw new JsonException("Ulid invalid: length must be 26");
                Span<byte> buf = stackalloc byte[26];
                seq.CopyTo(buf);
                Ulid.TryParse(buf, out var ulid);
                return ulid;
            }
            else
            {
                // Parse usign ValueSpan
                var buf = reader.ValueSpan;
                if (buf.Length != 26) throw new JsonException("Ulid invalid: length must be 26");
                Ulid.TryParse(buf, out var ulid);
                return ulid;
            }
        }
        catch (IndexOutOfRangeException e)
        {
            throw new JsonException("Ulid invalid: length must be 26", e);
        }
        catch (OverflowException e)
        {
            throw new JsonException("Ulid invalid: invalid character", e);
        }
    }

    /// <summary>
    /// Write a Ulid value as a lowercase string to JSON.
    /// Matches the format used by the Cysharp library.
    /// </summary>
    /// <remarks>
    /// Modified implementation to  ensure the output is in lowercase.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options)
    {
        Span<char> buf = stackalloc char[26];
        value.TryWriteStringify(buf);

        // Convert to lowercase without heap allocation (added code compared to original)
        // Also improvement compared to original for loop implementation of the modification, in terms of performance
        // Same as: MemoryExtensions.ToLowerInvariant(buf, buf);
        buf.ToLowerInvariant(buf);


        // Write the lowercase string to the JSON writer
        writer.WriteStringValue(buf);
    }
}
