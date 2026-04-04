using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Topics.UpdateTopic;

public record UpdateTopicRequest(
    // We do not support changing owner of topic for now.
    //[property: Required]
    //[property: StrictUlid(ErrorMessage = "UserId must be a valid, strict Crockford-formatted ULID.")]
    //string UserId,

    [property: Required]
    [property: MaxLength(255, ErrorMessage = "Name is too long.")]
    string Name,

    [property: MaxLength(1000, ErrorMessage = "Description is too long.")]
    string? Description,

    bool IsFavorite,

    [property: Required]
    [property: MaxLength(10, ErrorMessage = "Cannot have more than 10 tags.")]
    List<string> Tags);
