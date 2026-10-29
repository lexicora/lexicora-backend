using System.Text;
using System.Text.Json;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.UnitTests.Api;

public class RawJsonConverterTests
{
    private static readonly RawJsonConverter Converter = new();

    [Fact]
    public void Writes_json_text_verbatim_instead_of_as_a_string()
    {
        const string json = """{"level":2,"items":[1,true,null]}""";

        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            Converter.Write(writer, json, JsonSerializerOptions.Default);
        }

        Assert.Equal(json, Encoding.UTF8.GetString(stream.ToArray()));
    }

    [Fact]
    public void Reads_any_json_value_as_its_raw_text()
    {
        var reader = new Utf8JsonReader("""{"a":[1,2]}"""u8);
        reader.Read();

        Assert.Equal("""{"a":[1,2]}""", Converter.Read(ref reader, typeof(string), JsonSerializerOptions.Default));
    }
}
