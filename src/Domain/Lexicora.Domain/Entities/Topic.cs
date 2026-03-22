namespace Lexicora.Domain.Entities;

public class Topic
{
    public Guid Id { get; set; } = Ulid.New().ToGuid(); // Faster than Guid.CreateVersion7()
    public Guid UserId { get; set; }

    //TODO: Add slug later
    //potential slug schema: [{Name.ToLowerInvariant().Replace(" ", "-")}-{Ulid.New(Id).ToString()[..10]/*time part*/.ToLowerInvariant()}] 
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsFavorite { get; set; } = false;
    public List<string> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int EntryCount { get; set; } // Denormalized count of entries for performance

    // Navigation
    public User? User { get; set; }
    public List<Entry> Entries { get; set; } = [];
    //public ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
