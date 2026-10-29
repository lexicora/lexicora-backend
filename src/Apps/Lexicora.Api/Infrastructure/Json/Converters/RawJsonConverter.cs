using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lexicora.Api.Infrastructure.Json.Converters;

/// <summary>
/// Writes a string that already contains JSON (e.g. a Postgres <c>jsonb</c> column read as text) verbatim into the output.
/// </summary>
/// <remarks>
/// This avoids parsing the JSON into a <see cref="JsonDocument"/> just to serialize it again.
/// Input validation is skipped, because the value comes from a <c>jsonb</c> column, which Postgres guarantees to be valid JSON.
/// </remarks>
public class RawJsonConverter : JsonConverter<string>
{
    /// <inheritdoc/>
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        return document.RootElement.GetRawText();
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteRawValue(value, skipInputValidation: true);
}
