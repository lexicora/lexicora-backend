using System.Text.Json.Serialization;

using Lexicora.Api.Features.Entries.CreateEntry;
using Lexicora.Api.Features.Entries.GetAllEntries;
using Lexicora.Api.Features.Entries.GetEntriesByTopicId;
using Lexicora.Api.Features.Entries.GetEntryById;
using Lexicora.Api.Features.Entries.UpdateEntry;

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
[JsonSerializable(typeof(CreateEntryRequest))]
[JsonSerializable(typeof(CreateEntryResponse))]
[JsonSerializable(typeof(GetEntryByIdResponse))]
[JsonSerializable(typeof(List<GetEntriesByTopicIdResponse>))]
[JsonSerializable(typeof(GetAllEntriesResponseCollection))]
[JsonSerializable(typeof(UpdateEntryRequest))]
[JsonSerializable(typeof(UpdateEntryResponse))]
internal partial class EntriesJsonContext : JsonSerializerContext;
