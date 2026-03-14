using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Json.Converters;

using Microsoft.AspNetCore.Mvc;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    Converters = [typeof(LowercaseUlidJsonConverter)])]
[JsonSerializable(typeof(Ulid))]
//[JsonSerializable(typeof(NotFound))]
[JsonSerializable(typeof(ProblemDetails))]
internal partial class AppJsonContext : JsonSerializerContext;
