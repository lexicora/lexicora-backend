using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Topics.CreateTopic;

public static class CreateTopicMappers
{
    public static Topic MapToEntity(this CreateTopicRequest request)
    {
        var id = Ulid.New();

        return new Topic(
            id.ToGuid(),
            Ulid.Parse(request.UserId).ToGuid(), // Should be safe, since we validate this in the request validation.
            Utilities.CreateSlug(request.Name, id.ToString()),
            request.Name.Trim(),
            request.Description?.Trim(),
            request.Tags,
            request.IsFavorite);
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
