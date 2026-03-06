namespace Lexicora.Domain.Constants;

/// <summary>
/// Constants of every block type.
/// </summary>
public static class BlockTypes
{
    // Inline
    public const string Paragraph = "paragraph";
    public const string Heading = "heading";
    public const string Quote = "quote";
    public const string Blockquote = "blockquote";
    public const string BulletListItem = "bulletListItem";
    public const string NumberedListItem = "numberedListItem";
    public const string CheckListItem = "checkListItem";
    public const string ToggleListItem = "toggleListItem";
    public const string CodeBlock = "codeblock";

    // Embed / Contentless
    public const string File = "file";
    public const string Image = "image";
    public const string Video = "video";
    public const string Audio = "audio";

    public const string Divider = "divider";

    // Table
    public const string Table = "table";

    // Todo: Add more block types as needed
}
