namespace Lexicora.Domain.Constants;

/// <summary>Property values used in content blocks and inline elements. (usually in Props property)</summary>
public static class PropValues
{
    /// <summary>Default prop value for <see cref="PropNames.TextAlignment"/>.</summary>
    public const string Left = "left";

    /// <summary>Prop value for <see cref="PropNames.TextAlignment"/>.</summary>
    public const string Center = "center";

    /// <summary>Prop value for <see cref="PropNames.TextAlignment"/>.</summary>
    public const string Right = "right";

    /// <summary>Prop value for <see cref="PropNames.TextAlignment"/>.</summary>
    public const string Justify = "justify";

    public const string Default = "default";

    /// <summary>Prop value for <see cref="PropNames.ShowPreview"/>.</summary>
    public const bool ShowPreviewDefault = true;

    /// <summary>Prop value for <see cref="PropNames.PreviewWidth"/>.</summary>
    public const int PreviewWidthWithDefault = 512;

    /// <summary>Default prop value for <see cref="PropNames.Language"/>.</summary>
    public const string Text = "text";

    // Todo: Add every supported Programming language for code block as constant here. (name and alias) Reference: https://shiki.style/languages
}
