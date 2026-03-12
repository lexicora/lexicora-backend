using System.Text.Json.Serialization;

using Lexicora.Domain.Constants.Block;
using Lexicora.Domain.Contracts.BlockNote.Content;

namespace Lexicora.Domain.Entities.BlockNote.Content.Inline;

/// <summary>
/// Inline content base class.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Link), ContentTypes.Link)]
[JsonDerivedType(typeof(StyledText), ContentTypes.Text)]
public abstract class InlineContent : IBlockContent
{
    /// <summary>Type of inline content.</summary>
    [JsonIgnore]
    public abstract string Type { get; set; }
}
