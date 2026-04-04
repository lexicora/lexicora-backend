using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Topics.UpdateTopic;

public static class UpdateTopicMappers
{
    extension(Topic topic)
    {
        public UpdateTopicResponse MapToResponse() => new(
            Ulid.New(topic.Id),
            Ulid.New(topic.UserId),
            topic.Slug,
            topic.Name,
            topic.Description,
            topic.IsFavorite,
            topic.Tags,
            topic.CreatedAt,
            topic.UpdatedAt,
            topic.EntryCount);

        public void UpdateWithRequestData(UpdateTopicRequest request)
        {
            var topicId = Ulid.New(topic.Id);

            topic.Slug = Utilities.CreateSlug(request.Name, topicId.ToString());
            topic.Name = request.Name.Trim();
            topic.Description = request.Description?.Trim();
            topic.IsFavorite = request.IsFavorite;
            topic.Tags = request.Tags;
            topic.UpdatedAt = DateTime.UtcNow;
        }
    }
}
