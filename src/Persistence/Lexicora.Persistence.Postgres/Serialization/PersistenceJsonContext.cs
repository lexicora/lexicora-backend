using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lexicora.Persistence.Postgres.Serialization;

[JsonSourceGenerationOptions(
    WriteIndented = false, // compact JSON
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    GenerationMode = JsonSourceGenerationMode.Default // keep it flexible (can also use Metadata for smaller AOT size)
)]
[JsonSerializable(typeof(Dictionary<string, JsonElement>))] // props
public partial class PersistenceJsonContext : JsonSerializerContext;
