using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Entries.CreateEntry;
using Lexicora.Api.Features.Entries.DeleteEntry;
using Lexicora.Api.Features.Entries.GetAllEntries;
using Lexicora.Api.Features.Entries.GetEntriesByTopicId;
using Lexicora.Api.Features.Entries.GetEntryById;
using Lexicora.Api.Features.Entries.UpdateEntry;

namespace Lexicora.Api.Features.Entries;

public class EntriesConfiguration : IEndpointConfiguration
{
    public static void MapEndpoints(IEndpointRouteBuilder builder, bool? isDevelopment = null)
    {
        var entriesGroup = builder.MapGroup("/entries").WithTags("Entries");

        // CREATE (POST)
        CreateEntryEndpoint.Map(entriesGroup);

        // READ (GET)
        GetEntryByIdEndpoint.Map(entriesGroup);
        GetEntriesByTopicIdEndpoint.Map(entriesGroup);

        // UPDATE (PUT/PATCH)
        UpdateEntryEndpoint.Map(entriesGroup);

        // DELETE
        DeleteEntryEndpoint.Map(entriesGroup);

        // Development Endpoints
        if (isDevelopment is true)
        {
            GetAllEntriesEndpoint.Map(entriesGroup);
        }
    }
}
