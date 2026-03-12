using Lexicora.Domain.Constants.Block;

namespace Lexicora.Domain.Entities.BlockNote.Content.Inline;

/// <summary>
/// Represents a styled text element that contains text content and associated styles.
/// </summary>
/// <remarks>This class is used to define text content with specific styles, such as font, color, or other
/// formatting options. It is a sealed class and cannot be inherited.</remarks>
public sealed class StyledText : InlineContent //, IInlineNode
{
    public override string Type { get; set; } = ContentTypes.Text;

    /// <summary>The text content.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>The styles of the text.</summary>
    public Styles Styles { get; set; } = new(); // New Styles With default values
}
