using System.ComponentModel.DataAnnotations;

using Lexicora.Api.Features.Entries._Shared;

namespace Lexicora.Api.Features.Entries.UpdateEntry;

public record UpdateEntryRequest(
    // Moving an entry to another topic is not supported for now.

    [property: Required]
    [property: MaxLength(500, ErrorMessage = "Title is too long.")]
    string Title,

    [property: MaxLength(1000, ErrorMessage = "Description is too long.")]
    string? Description,

    bool IsFavorite,

    [property: Required]
    [property: MaxLength(10, ErrorMessage = "Cannot have more than 10 tags.")]
    List<string> Tags,

    /// <summary>
    /// The new content of the entry. When provided, it replaces the existing blocks entirely (like saving a document).
    /// When omitted, the content stays unchanged.
    /// </summary>
    List<FlatBlock>? Blocks = null);
