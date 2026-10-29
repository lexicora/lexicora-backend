using Lexicora.Domain.Common;
using Lexicora.Domain.Entities;

namespace Lexicora.Api.Features.Entries.UpdateEntry;

public static class UpdateEntryMappers
{
    extension(Entry entry)
    {
        public UpdateEntryResponse MapToResponse() => new(
            Ulid.New(entry.Id),
            Ulid.New(entry.UserId),
            Ulid.New(entry.TopicId),
            entry.Slug,
            entry.Title,
            entry.Description,
            entry.IsFavorite,
            entry.Tags,
            entry.CreatedAt,
            entry.UpdatedAt);

        public void UpdateWithRequestData(UpdateEntryRequest request)
        {
            var entryId = Ulid.New(entry.Id);

            entry.Slug = Utilities.CreateSlug(request.Title, entryId.ToString());
            entry.Title = request.Title.Trim();
            entry.Description = request.Description?.Trim();
            entry.IsFavorite = request.IsFavorite;
            entry.Tags = request.Tags;
            entry.UpdatedAt = DateTime.UtcNow;
        }
    }
}
