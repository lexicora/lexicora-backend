using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Infrastructure.Filters;
using Lexicora.Persistence.Postgres;

namespace Lexicora.Api.Features.Entries.CreateEntry;

public class CreateEntryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapPost("/", HandleAsync)
            .WithName("CreateEntry")
            .WithTags("Entries")
            .WithSummary("Create new entry")
            .WithDescription("Creates a new entry. The request body must include the required properties for creating an entry. Returns the created entry on success.")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<CreateEntryRequest>>();
    }

    private static async Task HandleAsync(
        AppDbContext dbContext, CancellationToken cancellationToken)
    {

    }
}
