namespace Lexicora.Domain.Contracts.BlockNote.Content;

/// <summary>
/// Interface for block content.
/// </summary>
public interface IBlockContent
{
    /// <summary>
    /// Type of block content.
    /// </summary>
    string Type { get; set; }
}
