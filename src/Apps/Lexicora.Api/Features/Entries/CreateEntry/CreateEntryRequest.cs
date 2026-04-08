using System.ComponentModel.DataAnnotations;

using Lexicora.Api.Features.Entries._Shared;
using Lexicora.Domain.Common.DataAnnotations;

namespace Lexicora.Api.Features.Entries.CreateEntry;

public record CreateEntryRequest(
    [property: StrictUlid(ErrorMessage = "TopicId must be a valid, strict Crockford-formatted ULID.")]
    string? TopicId,

    FlatTopic? Topic,

    [property: Required]
    [property: MaxLength(500, ErrorMessage = "Title is too long.")]
    string Title,

    [property: MaxLength(1000, ErrorMessage = "Description is too long.")]
    string? Description,

    [property: Required]
    [property: MaxLength(10, ErrorMessage = "Cannot have more than 10 tags.")]
    List<string> Tags,

    [property: MaxLength(1000, ErrorMessage = "FaviconUrl is too long.")]
    string? FaviconUrl,

    [property: Required]
    [property: MaxLength(1000, ErrorMessage = "Url is too long.")]
    string Url,

    [property: Required]
    [property: MaxLength(1000, ErrorMessage = "OriginUrl is too long.")]
    string OriginUrl,

    [property: MaxLength(1000, ErrorMessage = "PathUrl is too long.")]
    string? PathUrl,

    [property: Required]
    [property: MaxLength(10, ErrorMessage = "LanguageCode is too long.")]
    string LanguageCode,

    [property: MaxLength(255, ErrorMessage = "SiteName is too long.")]
    string? SiteName = null,

    [property: MaxLength(255, ErrorMessage = "Excerpt is too long.")]
    string? Excerpt = null,

    [property: MaxLength(255, ErrorMessage = "Byline is too long.")]
    string? Byline = null,

    DateTime? PublishedAt = null,

    bool IsFavorite = false,

    List<FlatBlock>? Blocks = null
    );
