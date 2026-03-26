namespace Lexicora.Api.Features.Topics.CreateTopic;

public record CreateTopicRequest(
    Ulid UserId,
    string Name,
    List<string> Tags,
    //bool IsFavorite, // leave out, can be set later by the user.
    string? Description = null);
