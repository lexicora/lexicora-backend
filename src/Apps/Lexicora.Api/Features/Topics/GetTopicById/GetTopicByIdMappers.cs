using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Topics.GetTopicById;

public static class GetTopicByIdMappers
{
    //extension (Topic topic) {}
    public static GetTopicByIdResponse MapToResponse(this Topic topic) => new(
            new Ulid(topic.Id),
            topic.Name,
            topic.Description,
            topic.IsFavorite,
            topic.Tags,
            topic.CreatedAt,
            topic.UpdatedAt,
            topic.EntryCount
        );
}
