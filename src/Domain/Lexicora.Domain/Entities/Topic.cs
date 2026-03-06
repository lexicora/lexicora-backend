namespace Lexicora.Domain.Entities;

public class Topic
{
    public Guid Id { get; set; } = Ulid.NewUlid().ToGuid(); // Faster than Guid.CreateVersion7()
    public Guid UserId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Tags { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public int EntryCount { get; set; } // Denormalized count of entries for performance

    // Navigation
    public User? User { get; set; }
    public List<Entry> Entries { get; set; } = [];
    //public ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
