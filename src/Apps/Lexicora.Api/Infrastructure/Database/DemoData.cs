using System.Text.Json;

using Lexicora.Api.Features.Entries._Shared;
using Lexicora.Api.Infrastructure.Json.Contexts;
using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;
using Lexicora.Persistence.Postgres;

using Microsoft.EntityFrameworkCore;

namespace Lexicora.Api.Infrastructure.Database;

/// <summary>
/// Sample data for the embedded database, so the API has something to show right after startup.
/// </summary>
public static class DemoData
{
    // A document in BlockNote's format, as the extension's editor would send it.
    private const string SampleDocument = """
        [
          {
            "type": "heading",
            "props": { "level": 2, "textColor": "default", "backgroundColor": "default", "textAlignment": "left", "isToggleable": false },
            "content": [{ "type": "text", "text": "Notes on the Analytical Engine", "styles": {} }],
            "children": []
          },
          {
            "type": "paragraph",
            "props": { "textColor": "default", "backgroundColor": "default", "textAlignment": "left" },
            "content": [
              { "type": "text", "text": "The Analytical Engine ", "styles": {} },
              { "type": "text", "text": "weaves algebraic patterns", "styles": { "italic": true } },
              { "type": "text", "text": " just as the Jacquard loom weaves flowers and leaves.", "styles": {} }
            ],
            "children": []
          },
          {
            "type": "bulletListItem",
            "props": { "textColor": "default", "backgroundColor": "default", "textAlignment": "left" },
            "content": [{ "type": "text", "text": "Components", "styles": { "bold": true } }],
            "children": [
              {
                "type": "bulletListItem",
                "props": { "textColor": "default", "backgroundColor": "default", "textAlignment": "left" },
                "content": [{ "type": "text", "text": "The store (memory)", "styles": {} }],
                "children": []
              },
              {
                "type": "bulletListItem",
                "props": { "textColor": "default", "backgroundColor": "default", "textAlignment": "left" },
                "content": [{ "type": "text", "text": "The mill (processor)", "styles": {} }],
                "children": []
              }
            ]
          },
          {
            "type": "paragraph",
            "props": { "textColor": "default", "backgroundColor": "default", "textAlignment": "left" },
            "content": [
              { "type": "text", "text": "Source: ", "styles": {} },
              { "type": "link", "href": "https://en.wikipedia.org/wiki/Analytical_engine", "content": [{ "type": "text", "text": "Wikipedia", "styles": {} }] }
            ],
            "children": []
          }
        ]
        """;

    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Users.AnyAsync(cancellationToken)) return;

        var user = new User(Ulid.New().ToGuid(), "ada@example.com", "ada.lovelace", "Ada", "Lovelace", "Countess of Lovelace");

        var history = CreateTopic(user, "History of Computing", "Early machines and the people behind them.", ["history", "computing"]);
        var reading = CreateTopic(user, "Reading List", null, ["to-read"]);

        var entryId = Ulid.New();
        var entry = new Entry(
            entryId.ToGuid(),
            user.Id,
            history.Id,
            Utilities.CreateSlug("Analytical Engine", entryId.ToString()),
            "Analytical Engine",
            "Babbage's proposed general-purpose computer.",
            ["babbage", "machines"],
            "https://en.wikipedia.org/favicon.ico",
            "https://en.wikipedia.org/wiki/Analytical_engine",
            "https://en.wikipedia.org",
            "/wiki/Analytical_engine",
            "en",
            "Wikipedia",
            "The Analytical Engine was a proposed digital mechanical general-purpose computer designed by Charles Babbage.",
            null,
            null,
            isFavorite: true);

        var document = JsonSerializer.Deserialize(SampleDocument, EntriesJsonContext.Default.ListFlatBlock)!;
        entry.Blocks = document.MapToEntities(entry.Id);

        history.EntryCount = 1;
        user.TopicCount = 2;
        user.EntryCount = 1;

        dbContext.AddRange(user, history, reading, entry);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Topic CreateTopic(User user, string name, string? description, List<string> tags)
    {
        var id = Ulid.New();
        return new Topic(id.ToGuid(), user.Id, Utilities.CreateSlug(name, id.ToString()), name, description, tags);
    }
}
