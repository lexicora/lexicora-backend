using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lexicora.Api.Infrastructure.Json.Converters;

/// <summary>
/// A custom JSON converter for the <see cref="Ulid"/> type.
/// </summary>
/// <remarks>
/// This converter ensures that ULIDs are always serialized as lowercase strings.
/// It is slightly modified, from the default implementation of ByteAether's implementation, which this is originally derived from.
/// </remarks>
public class LowercaseUlidJsonConverter : JsonConverter<Ulid>
{
    /// <inheritdoc/>
    public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            if (reader.TokenType is not JsonTokenType.String and not JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected string or property name");
            }

            if (reader.HasValueSequence)
            {
                var byteSequence = reader.ValueSequence;
                if (byteSequence.Length != Ulid.UlidStringLength)
                {
                    throw new JsonException($"Ulid invalid: length must be {Ulid.UlidStringLength}");
                }

                Span<byte> byteSpan = stackalloc byte[Ulid.UlidStringLength];
                byteSequence.CopyTo(byteSpan);
                return Ulid.TryParse(byteSpan, null, out var ulid)
                    ? ulid
                    : throw new JsonException("Ulid invalid: invalid character");
            }
            else
            {
                var byteSpan = reader.ValueSpan;
                if (byteSpan.Length != Ulid.UlidStringLength)
                {
                    throw new JsonException($"Ulid invalid: length must be {Ulid.UlidStringLength}");
                }

                return Ulid.TryParse(byteSpan, null, out var ulid)
                    ? ulid
                    : throw new JsonException("Ulid invalid: invalid character");
            }
        }
        catch (IndexOutOfRangeException ex)
        {
            throw new JsonException($"Ulid invalid: length must be {Ulid.UlidStringLength}", ex);
        }
        catch (OverflowException ex)
        {
            throw new JsonException("Ulid invalid: invalid character", ex);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Ulid ulid, JsonSerializerOptions options)
    {
        Span<byte> ulidString = stackalloc byte[Ulid.UlidStringLength];
        ulid.TryFormat(ulidString, out var _, []);

        // Added code, compared to original implementation.
        Span<byte> lowerBuffer = stackalloc byte[Ulid.UlidStringLength];
        Ascii.ToLower(ulidString, lowerBuffer, out _);

        writer.WriteStringValue(lowerBuffer);
    }

    /// <inheritdoc/>
    public override void WriteAsPropertyName(Utf8JsonWriter writer, Ulid ulid, JsonSerializerOptions options)
    {
        Span<byte> ulidString = stackalloc byte[Ulid.UlidStringLength];
        ulid.TryFormat(ulidString, out var _, []);

        // Added code, compared to original implementation.
        Span<byte> lowerBuffer = stackalloc byte[Ulid.UlidStringLength];
        Ascii.ToLower(ulidString, lowerBuffer, out _);

        writer.WritePropertyName(lowerBuffer);
    }

    /// <inheritdoc/>
    public override Ulid ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Read(ref reader, typeToConvert, options);
}
