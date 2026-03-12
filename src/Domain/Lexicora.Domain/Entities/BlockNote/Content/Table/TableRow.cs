namespace Lexicora.Domain.Entities.BlockNote.Content.Table;

/// <summary>
/// Represents a row in a table, containing a list of table cells.
/// </summary>
public sealed class TableRow
{
    public List<TableCell> Cells { get; set; } = [];
}
