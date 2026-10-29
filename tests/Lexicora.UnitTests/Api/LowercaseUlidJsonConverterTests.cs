using System.Text;
using System.Text.Json;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.UnitTests.Api;

public class LowercaseUlidJsonConverterTests
{
    private static readonly LowercaseUlidJsonConverter Converter = new();
    private static readonly Ulid SampleUlid = Ulid.Parse("01ARZ3NDEKTSV4RRFFQ69G5FAV");

    [Fact]
    public void Writes_ulid_as_lowercase_string()
    {
        var json = Write(writer => Converter.Write(writer, SampleUlid, JsonSerializerOptions.Default));

        Assert.Equal("\"01arz3ndektsv4rrffq69g5fav\"", json);
    }

    [Fact]
    public void Writes_property_names_as_lowercase()
    {
        var json = Write(writer =>
        {
            writer.WriteStartObject();
            Converter.WriteAsPropertyName(writer, SampleUlid, JsonSerializerOptions.Default);
            writer.WriteNumberValue(1);
            writer.WriteEndObject();
        });

        Assert.Equal("{\"01arz3ndektsv4rrffq69g5fav\":1}", json);
    }

    [Theory]
    [InlineData("01ARZ3NDEKTSV4RRFFQ69G5FAV")]
    [InlineData("01arz3ndektsv4rrffq69g5fav")]
    public void Reads_ulid_regardless_of_casing(string value)
    {
        Assert.Equal(SampleUlid, Read($"\"{value}\""));
    }

    [Theory]
    [InlineData("\"01ARZ3NDEKTSV4RRFFQ69G5FA\"")] // Too short
    [InlineData("\"01ARZ3NDEKTSV4RRFFQ69G5FA!\"")] // Invalid character
    [InlineData("42")] // Not a string
    public void Rejects_invalid_values_with_json_exception(string json)
    {
        Assert.Throws<JsonException>(() => Read(json));
    }

    private static string Write(Action<Utf8JsonWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            write(writer);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static Ulid Read(string json)
    {
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        reader.Read();
        return Converter.Read(ref reader, typeof(Ulid), JsonSerializerOptions.Default);
    }
}
