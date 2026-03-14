namespace Lexicora.Api.Features.Topics.GetTopicById;

public record GetTopicByIdResponse(
    Ulid Id,
    string Name,
    string? Description,
    bool IsFavorite,
    List<string> Tags,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    int EntryCount
);
