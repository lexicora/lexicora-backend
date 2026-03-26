namespace Lexicora.Api.Features.Topics.CreateTopic;

public record CreateTopicResponse(
    Ulid Id,
    Ulid UserId,
    string Slug,
    string Name,
    string? Description,
    List<string> Tags,
    bool IsFavorite, // Maybe leave out for this response, since always false on creation
    DateTime CreatedAt,
    int EntryCount);
