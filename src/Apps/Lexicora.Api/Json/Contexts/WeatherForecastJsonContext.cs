using System.Text.Json.Serialization;

namespace Lexicora.Api.Json.Contexts;

/// <summary>
/// Source-generated JSON context for serializing and deserializing <see cref="Lexicora.Api.WeatherForecast"/>.
/// </summary>
/// <remarks>
/// This context is used to optimize JSON serialization and deserialization for the <see cref="Lexicora.Api.WeatherForecast"/> class.
/// This should serve as an example of how to create a source-generated JSON context for the application.
/// </remarks>
#if DEBUG
[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase /*or PascalCase*/,
    GenerationMode = JsonSourceGenerationMode.Default)]
#else
[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase /*or PascalCase*/,
    GenerationMode = JsonSourceGenerationMode.Default)]
#endif
[JsonSerializable(typeof(WeatherForecast))]
[JsonSerializable(typeof(WeatherForecast[]))]
[JsonSerializable(typeof(List<WeatherForecast>))]
internal partial class WeatherForecastJsonContext : JsonSerializerContext;
