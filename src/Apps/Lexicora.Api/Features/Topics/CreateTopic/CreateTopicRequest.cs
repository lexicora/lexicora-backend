using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Topics.CreateTopic;

public record CreateTopicRequest(
    [property: Required]
    Ulid UserId,

    [property: Required]
    [property: MaxLength(255, ErrorMessage = "Name is too long.")]
    string Name,

    [property: Required]
    [property: MaxLength(10, ErrorMessage = "Cannot have more than 10 tags.")]
    List<string> Tags,
    //bool IsFavorite, // leave out, can be set later by the user.

    [property: MaxLength(1000, ErrorMessage = "Description is too long.")]
    string? Description = null);
