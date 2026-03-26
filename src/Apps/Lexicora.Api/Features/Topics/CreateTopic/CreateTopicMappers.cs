using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Topics.CreateTopic;

public static class CreateTopicMappers
{
    public static Topic MapToEntity(this CreateTopicRequest request) => new()
    {
        Id = Ulid.New().ToGuid(),
        UserId = request.UserId.ToGuid(),
        Slug = request.Name.ToLowerInvariant().Replace(" ", "-").Trim(),
        Name = request.Name.Trim(),
        Description = request.Description?.Trim(),
        Tags = request.Tags,
        IsFavorite = false,
        CreatedAt = DateTime.UtcNow,
        EntryCount = 0
    };

    public static CreateTopicResponse MapToResponse(this Topic topic) => new(
            Ulid.New(topic.Id),
            Ulid.New(topic.UserId),
            topic.Slug,
            topic.Name,
            topic.Description,
            topic.Tags,
            topic.IsFavorite,
            topic.CreatedAt,
            topic.EntryCount
        );
}
