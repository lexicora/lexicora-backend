namespace Lexicora.Domain.Entities;

public class User()
{
    public User(Guid id, string email, string username, string firstName, string lastName, string? displayName = null)
        : this()
    {
        Id = id;
        Email = email;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; } // = Ulid.NewUlid().ToGuid(); // Matches Supabase Auth UUID, let supabase handle id gen?
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int EntryCount { get; set; } // Denormalized count of entries for performance
    public int TopicCount { get; set; } // Denormalized count of topics for performance

    // Navigation
    public List<Topic> Topics { get; set; } = [];
    public List<Entry> Entries { get; set; } = [];
}
