namespace Lexicora.Domain.Entities;

public class Entry
{
    public Guid Id { get; set; } = Ulid.New().ToGuid(); // Faster than Guid.CreateVersion7()
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }

    // Page Data (From your extension's PageData type)
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Tags { get; set; } = [];
    public bool IsFavorite { get; set; } = false;
    public string? FaviconUrl { get; set; }
    public string Url { get; set; } = string.Empty; // maps to location.href or baseUri
    public string OriginUrl { get; set; } = string.Empty; // The original URL before any redirects (if available)
    public string? PathUrl { get; set; }
    public string LanguageCode { get; set; } = string.Empty; // Language code (e.g., "en", "fr")
    public DateTimeOffset CreatedAt { get; set; } // When you scraped it
    public DateTimeOffset? UpdatedAt { get; set; } // When you last updated it

    // Metadata
    public string? SiteName { get; set; }
    public string? Excerpt { get; set; }
    public string? Byline { get; set; } // Author or source of the content
    public DateTimeOffset? PublishedAt { get; set; } // When the article was published

    // Navigation
    public User? User { get; set; }
    public Topic? Topic { get; set; }
    public List<Block> Blocks { get; set; } = []; // TODO: Add Blocks later when we have the Block entity defined

    //public ICollection<BlockBase> Blocks { get; set; } = new List<Block>(); // TODO: Add Blocks later when we have the Block entity defined
}
