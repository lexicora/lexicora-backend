using System.Text.Json.Serialization;

using Lexicora.Api.Features.Topics.GetTopicById;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

#if DEBUG
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
#else
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = false)]
#endif
[JsonSerializable(typeof(GetTopicByIdResponse))]
//[JsonSerializable(typeof(Ok<GetTopicByIdResponse>))]
internal partial class TopicsJsonContext : JsonSerializerContext;
