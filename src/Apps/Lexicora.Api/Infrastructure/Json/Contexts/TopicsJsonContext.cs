using System.Text.Json.Serialization;

using Lexicora.Api.Features.Topics.CreateTopic;
using Lexicora.Api.Features.Topics.GetAllTopics;
using Lexicora.Api.Features.Topics.GetTopicById;
using Lexicora.Api.Features.Topics.GetTopicBySlug;
using Lexicora.Api.Features.Topics.GetTopicsByUserId;
using Lexicora.Api.Features.Topics.UpdateTopic;

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
[JsonSerializable(typeof(CreateTopicRequest))]
[JsonSerializable(typeof(CreateTopicResponse))]
[JsonSerializable(typeof(GetTopicByIdResponse))]
[JsonSerializable(typeof(GetTopicBySlugResponse))]
[JsonSerializable(typeof(UpdateTopicRequest))]
[JsonSerializable(typeof(UpdateTopicResponse))]
[JsonSerializable(typeof(List<GetTopicsByUserIdResponse>))]
[JsonSerializable(typeof(GetAllTopicsResponseCollection))]
//[JsonSerializable(typeof(Ok<GetTopicByIdResponse>))]
internal partial class TopicsJsonContext : JsonSerializerContext;
