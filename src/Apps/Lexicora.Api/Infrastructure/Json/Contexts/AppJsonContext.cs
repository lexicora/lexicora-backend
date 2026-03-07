using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    Converters = [typeof(LowercaseUlidJsonConverter)])]
[JsonSerializable(typeof(Ulid))]
internal partial class AppJsonContext : JsonSerializerContext;
