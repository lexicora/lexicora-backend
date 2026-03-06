using Lexicora.Domain.Constants;

namespace Lexicora.Domain.Entities.Block.Content.Inline;

/// <summary>
/// Represents a hyperlink element that contains link content and an associated URL (href).
/// </summary>
public sealed class Link : InlineContent //, IInlineNode
{
    public override string Type { get; set; } = ContentTypes.Link;

    /// <summary>The content of the link.</summary>
    public List<StyledText> Content { get; set; } = [];

    /// <summary>The href of the link.</summary>
    public string Href { get; set; } = string.Empty;
}
