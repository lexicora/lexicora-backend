using System.Text.Json.Serialization;

namespace Lexicora.Domain.Entities.Block.Content.Inline;

//Use this if the json behavior below is not needed
#region Not used

///// <summary>
///// The default values for all booleans is <c>false</c>.
///// The default value for TextColor is <c>"default"</c>.
///// </summary>
///// <param name="Bold"></param>
///// <param name="Italic"></param>
///// <param name="Underline"></param>
///// <param name="Strike"></param>
///// <param name="TextColor"></param>
//public readonly record struct Styles(
//    bool Bold = false,
//    bool Italic = false,
//    bool Underline = false,
//    bool Strike = false,
//    string TextColor = "default"
//);

#endregion

// Info: Use this below if needed (BlockNote suggests this behavior)

/// <summary>
/// Styles Object for StyledText.
/// The default values for all booleans is <c>false</c>.
/// The default value for TextColor is <c>"default"</c>.
/// </summary>
public readonly record struct Styles()
{
    /// <summary>Whether the text is bold. Default: false</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Bold { get; init; } = false;

    /// <summary>Whether the text is italic. Default: false</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Italic { get; init; } = false;

    /// <summary>Whether the text is underlined. Default: false</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Underline { get; init; } = false;

    /// <summary>Whether the text is struck through. Default: false</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Strike { get; init; } = false;

    /// <summary>The text color. Default: "default"</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string TextColor { get; init; } = "default";
}
