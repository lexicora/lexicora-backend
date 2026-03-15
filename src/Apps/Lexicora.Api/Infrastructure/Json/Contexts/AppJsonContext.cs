using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Mvc;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
//[JsonSerializable(typeof(Ulid))]
//[JsonSerializable(typeof(NotFound))]
[JsonSerializable(typeof(ProblemDetails))]
internal partial class AppJsonContext : JsonSerializerContext;
