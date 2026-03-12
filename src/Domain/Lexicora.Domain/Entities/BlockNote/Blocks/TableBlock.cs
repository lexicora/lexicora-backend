using System.Text.Json.Serialization;

using Lexicora.Domain.Entities.BlockNote.Content.Table;

namespace Lexicora.Domain.Entities.BlockNote.Blocks;

/// <summary>
/// Table block type, derives from <see cref="BlockBase"/>.
/// </summary>
public sealed class TableBlock : BlockBase
{
    [JsonPropertyName("content")]
    public TableContent Content { get; set; } = new();

    // Todo: Add create methods with predefined configurations.
}
