using System.Text.Json;

using Lexicora.Domain.Constants.Block;
using Lexicora.Domain.Entities.BlockNote.Content.Inline;

namespace Lexicora.Domain.Entities.BlockNote.Content.Table;

/// <summary>
/// Represents a cell within a table row, containing inline content.
/// </summary>
public sealed class TableCell //: IContent
{
    public string Type { get; set; } = ContentTypes.TableCell;

    public Dictionary<string, JsonElement>? Props { get; set; }

    public List<InlineContent> Content { get; set; } = []; // Use abstract class InlineContent or interface IInlineNode
}
