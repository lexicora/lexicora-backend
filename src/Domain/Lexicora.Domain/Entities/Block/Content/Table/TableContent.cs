using Lexicora.Domain.Constants;
using Lexicora.Domain.Contracts.Block.Content;

namespace Lexicora.Domain.Entities.Block.Content.Table;

/// <summary>
/// Represents a table structure with rows and columns.
/// </summary>
public sealed class TableContent : IBlockContent
{
    public string Type { get; set; } = ContentTypes.TableContent;

    /// <summary>Default for creating a new table is 3 times null.</summary>
    public List<int?> ColumnWidths { get; set; } = []; // default for each column is null, meaning auto width should count should match with header rows

    /// <summary>Default is 3 header rows.</summary>
    public int HeaderRows { get; set; } // default 3 header rows (header rows are disabled in table by default)

    // TODO: Add potential HeaderColumns property.

    /// <summary>Default is 2 table rows with, with each table row having 3 table cells.</summary>
    public List<TableRow> Rows { get; set; } = [];
}
