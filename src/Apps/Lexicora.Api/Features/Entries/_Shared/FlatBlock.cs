using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Lexicora.Api.Features.Entries._Shared;

public record FlatBlock(
    string? Id,

    [property: Required]
    string Type,

    [property: Required]
    JsonElement PropsJson,

    [property: Required]
    List<FlatBlock> Children,

    JsonElement? ContentJson = null,
    double? Order = null);
