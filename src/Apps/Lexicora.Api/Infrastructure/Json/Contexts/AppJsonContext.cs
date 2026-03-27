using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
//[JsonSerializable(typeof(Ulid))]
//[JsonSerializable(typeof(NotFound))]
[JsonSerializable(typeof(ProblemHttpResult))]
internal partial class AppJsonContext : JsonSerializerContext;
