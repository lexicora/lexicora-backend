using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Topics.CreateTopic;

public static class CreateTopicMappers
{
    public static Topic MapToEntity(this CreateTopicRequest request)
    {
        var id = Ulid.New();

        return new Topic
        {
            Id = id.ToGuid(),
            UserId = request.UserId.ToGuid(),
            Slug = Utilities.CreateSlug(request.Name, id.ToString()),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Tags = request.Tags,
            //IsFavorite = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CreateTopicResponse MapToResponse(this Topic topic) => new(
            Ulid.New(topic.Id),
            Ulid.New(topic.UserId),
            topic.Slug,
            topic.Name,
            topic.Description,
            topic.Tags,
            topic.IsFavorite,
            topic.CreatedAt
        );
}
