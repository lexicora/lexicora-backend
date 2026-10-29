using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lexicora.Api.Features.Entries._Shared;

/// <summary>
/// A block as sent by the BlockNote editor. The JSON shape matches BlockNote's own document format,
/// so the editor's document can be posted as-is.
/// </summary>
public record FlatBlock(
    string? Id,

    [property: Required]
    string Type,

    [property: JsonPropertyName("props")]
    JsonElement PropsJson,

    [property: Required]
    List<FlatBlock> Children,

    [property: JsonPropertyName("content")]
    JsonElement? ContentJson = null,

    double? Order = null);
