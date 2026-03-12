using System.Text.Json.Serialization;

using Lexicora.Domain.Entities.BlockNote.Content.Inline;

namespace Lexicora.Domain.Entities.BlockNote.Blocks;

/// <summary>
/// Inline block type, derives from <see cref="BlockBase"/>.
/// </summary>
public sealed class InlineBlock : BlockBase
{
    [JsonPropertyName("content")]
    public List<InlineContent> Content { get; set; } = [];

    // Todo: Add create methods with predefined configurations.
}
