using System.Text.Json;
using System.Text.Json.Serialization;

using Lexicora.Domain.Constants.Block;

namespace Lexicora.Domain.Entities.BlockNote.Blocks;

/// <summary>
/// Base class of the Block entity.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.Paragraph)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.Heading)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.Quote)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.Blockquote)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.BulletListItem)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.NumberedListItem)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.CheckListItem)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.ToggleListItem)]
[JsonDerivedType(typeof(InlineBlock), BlockTypes.CodeBlock)]
[JsonDerivedType(typeof(VoidBlock), BlockTypes.File)]
[JsonDerivedType(typeof(VoidBlock), BlockTypes.Image)]
[JsonDerivedType(typeof(VoidBlock), BlockTypes.Video)]
[JsonDerivedType(typeof(VoidBlock), BlockTypes.Audio)]
[JsonDerivedType(typeof(VoidBlock), BlockTypes.Divider)]
[JsonDerivedType(typeof(TableBlock), BlockTypes.Table)]
public abstract class BlockBase
{
    [JsonIgnore]
    public Guid EntryId { get; set; }

    [JsonIgnore]
    public Guid? ParentBlockId { get; set; }

    [JsonIgnore]
    public Entry? Entry { get; set; }

    [JsonIgnore]
    public BlockBase? ParentBlock { get; set; } // Parent for nested blocks

    [JsonIgnore]
    public double Order { get; set; } // Order in document (nested blocks should have their own order and not increment the top level block order)

    /// <summary>
    /// Gets or sets the JSON representation of the content.
    /// </summary>
    /// <remarks>This property is ignored during JSON serialization when the condition is set to always
    /// ignore. It is intended to hold the content in JSON format, which can be used for data interchange or
    /// storage.</remarks>
    [JsonIgnore]
    public string? ContentJson { get; set; }

    // Actual Block
    /// <summary>The id of the actual Block, used by BlockNote and used as id in the database.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("props")]
    public Dictionary<string, JsonElement> Props { get; set; } = new();

    [JsonPropertyName("children")]
    public List<BlockBase> Children { get; set; } = [];

    // Todo: Add create methods with predefined configurations or to create and other.
}
