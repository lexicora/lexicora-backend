using System.Text.Json.Serialization;

using Lexicora.Api.Json.Converters;

namespace Lexicora.Api.Json.Contexts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    Converters = [typeof(LowercaseUlidJsonConverter)])]
[JsonSerializable(typeof(Ulid))]
internal partial class AppJsonContext : JsonSerializerContext;
