namespace Lexicora.Domain.Entities;

public class Topic
{
    public Guid Id { get; set; } = Ulid.New().ToGuid(); // Faster than Guid.CreateVersion7()
    public Guid UserId { get; set; }

    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Tags { get; set; } = new(10); // max 50 chars per tag, max 10 tags per entry (db enforced)
    public bool IsFavorite { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int EntryCount { get; set; } // Denormalized count of entries for performance

    // Navigation
    public User? User { get; set; }
    public List<Entry> Entries { get; set; } = [];
    //public ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
