using System.Text.Json.Serialization;

using Lexicora.Api.Infrastructure.Common;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
//[JsonSerializable(typeof(Ulid))]
//[JsonSerializable(typeof(NotFound))]
[JsonSerializable(typeof(ProblemHttpResult))]
[JsonSerializable(typeof(BadRequestResponse))]
internal partial class AppJsonContext : JsonSerializerContext;
