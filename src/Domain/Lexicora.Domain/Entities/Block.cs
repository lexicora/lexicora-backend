namespace Lexicora.Domain.Entities;

public class Block
{
    public Guid Id { get; set; } // = Ulid.NewUlid().ToGuid(); // Part of actual BlockNote schema.
    public Guid EntryId { get; set; }
    public Guid? ParentBlockId { get; set; }

    public double Order { get; set; }
    public required string Type { get; set; } = string.Empty; // Part of actual BlockNote schema.
    public string PropsJson { get; set; } = "{}"; // Part of actual BlockNote schema.
    public string? ContentJson { get; set; } // Part of actual BlockNote schema.


    // Navigation
    public Entry? Entry { get; set; }
    public Block? ParentBlock { get; set; }
    public List<Block> Children { get; set; } = []; // Part of actual BlockNote schema.
}
