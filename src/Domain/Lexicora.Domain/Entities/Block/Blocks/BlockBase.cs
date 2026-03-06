using System.Text.Json;
using System.Text.Json.Serialization;

using Lexicora.Domain.Constants;

namespace Lexicora.Domain.Entities.Block.Blocks;

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
    // From Base (not exposed in API) // Maybe expose in api (for just one joined Id).
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Guid Id { get; set; } // Internal DB Id, not serialized in API

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Guid EntryId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Guid? ParentBlockId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Entry? Entry { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public BlockBase? ParentBlock { get; set; } // Parent for nested blocks

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public int Order { get; set; } // Order in document (including/excluding? nested)

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public int NestLevel { get; set; } // Default 0 means top level

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public int? OrderNested { get; set; }

    ///// <summary>The nest level of the child, child... block of this block.</summary>
    //[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    //public int DeepestChildBlockNestLevel { get; set; }

    // Actual Block
    /// <summary>The id of the actual Block, used by BlockNote.</summary>
    [JsonPropertyName("id")]
    public Guid BlockId { get; set; } = Guid.NewGuid();// UUIDv4

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("props")]
    public Dictionary<string, JsonElement> Props { get; set; } = new();

    // Info: Derived classes should implement a Content property.
    // Content implemented in derived classes of BlockBase

    [JsonPropertyName("children")]
    public List<BlockBase> Children { get; set; } = [];

    // Todo: Add create methods with predefined configurations or to create and other.
}
