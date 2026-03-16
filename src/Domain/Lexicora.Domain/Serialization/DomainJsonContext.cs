namespace Lexicora.Domain.Serialization;

//[JsonSourceGenerationOptions(
//    WriteIndented = false, // compact JSON
//    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
//    //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
//    GenerationMode = JsonSourceGenerationMode.Default // keep it flexible (can also use Metadata for smaller AOT size)
//)]
//[JsonSerializable(typeof(string))] // In a way unnecessary
//[JsonSerializable(typeof(JsonElement))] // In a way unnecessary
////[JsonSerializable(typeof(Dictionary<string, JsonElement>))] // props
public partial class DomainJsonContext; /*: JsonSerializerContext;*/
