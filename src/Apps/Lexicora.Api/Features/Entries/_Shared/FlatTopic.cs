using System.ComponentModel.DataAnnotations;

using Lexicora.Domain.Common.DataAnnotations;

namespace Lexicora.Api.Features.Entries._Shared;

public record FlatTopic(
    [property: Required]
    [property: StrictUlid(ErrorMessage = "UserId must be a valid, strict Crockford-formatted ULID.")]
    string UserId,

    [property: Required]
    [property: MaxLength(255, ErrorMessage = "Name is too long.")]
    string Name,

    // Not needed in this request scope.
    //[property: Required]
    //[property: MaxLength(10, ErrorMessage = "Cannot have more than 10 tags.")]
    //List<string> Tags,

    bool IsFavorite = false, // leave out, can be set later by the user.

    [property: MaxLength(1000, ErrorMessage = "Description is too long.")]
    string? Description = null);
