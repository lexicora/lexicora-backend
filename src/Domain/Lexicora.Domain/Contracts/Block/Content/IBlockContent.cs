namespace Lexicora.Domain.Contracts.Block.Content;

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
