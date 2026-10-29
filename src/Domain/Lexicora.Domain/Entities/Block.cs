using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Lexicora.Domain.Entities;

public class Block()
{
    // Parsed once, JsonElement is an immutable value, so the instance can be shared. Avoids reflection based serialization.
    private static readonly JsonElement EmptyProps = JsonElement.Parse("{}");

    [SetsRequiredMembers]
    public Block(Guid id, Guid entryId, Guid? parentBlockId,
        double order, string type, JsonElement propsJson, JsonElement? contentJson,
        List<Block>? children = null) : this()
    {
        Id = id;
        EntryId = entryId;
        ParentBlockId = parentBlockId;

        Order = order;
        Type = type;
        PropsJson = propsJson;
        ContentJson = contentJson;
        Children = children ?? [];
    }

    public Guid Id { get; set; } // = Ulid.NewUlid().ToGuid(); // Part of actual BlockNote schema.
    public Guid EntryId { get; set; }
    public Guid? ParentBlockId { get; set; }

    public double Order { get; set; }
    public required string Type { get; set; } = string.Empty; // Part of actual BlockNote schema.
    public JsonElement PropsJson { get; set; } = EmptyProps; // Part of actual BlockNote schema.
    public JsonElement? ContentJson { get; set; } // Part of actual BlockNote schema.


    // Navigation
    public Entry? Entry { get; set; }
    public Block? ParentBlock { get; set; }
    public List<Block> Children { get; set; } = []; // Part of actual BlockNote schema.
}
