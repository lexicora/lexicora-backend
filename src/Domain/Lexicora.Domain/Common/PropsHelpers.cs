using System.Text.Json;

using Lexicora.Domain.Constants;

namespace Lexicora.Domain.Common;

/// <summary>
/// This static helper/utility class contains useful methods to easily create or add needed prop values, for Props in various types.
/// </summary>
public static class PropsHelpers
{
    #region Not used

    /*
        *type DefaultProps = {
         /**
          * The background color of the block, which also applies to nested blocks.
          * @default "default"
          * /
         backgroundColor: string;
         /**
          * The text color of the block, which also applies to nested blocks.
          * @default "default"
          * /
         textColor: string;
         /**
          * The text alignment of the block.
          * @default "left"
          * /
         textAlignment: "left" | "center" | "right" | "justify";
        };
    */

    #endregion

    /// <summary>
    /// Adds the default values to a props dictionary.
    /// </summary>
    /// <remarks>
    /// If you want to create or add custom values to your dictionary, call the given custom add props extension method before this.
    /// </remarks>
    /// <param name="props"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Returns modified props dictionary.</returns>
    public static Dictionary<string, JsonElement> AddDefaultProps(this Dictionary<string, JsonElement> props,
       string backgroundColor = PropValues.Default,
       string textColor = PropValues.Default,
       string textAlignment = PropValues.Left)
    {
        props[PropNames.BackgroundColor] = JsonSerializer.SerializeToElement(backgroundColor);
        props[PropNames.TextColor] = JsonSerializer.SerializeToElement(textColor);
        props[PropNames.TextAlignment] = JsonSerializer.SerializeToElement(textAlignment);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values of the DefaultProps.
    /// </summary>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with DefaultProps.</returns>
    public static Dictionary<string, JsonElement> CreateDefaultProps(
        string backgroundColor = PropValues.Default,
        string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>().AddDefaultProps(backgroundColor, textColor, textAlignment);

    // Text (Heading)

    /// <summary>
    /// Adds HeadingBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="headingLevel"></param>
    /// <param name="isToggleable"></param>
    /// <returns>Dictionary with HeadingBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddHeadingBlockProps(this Dictionary<string, JsonElement> props, int headingLevel = 1, bool isToggleable = false)
    {
        // Accepted values 1, 2, 3
        if (headingLevel is > 3 or < 1) headingLevel = 1;

        props[PropNames.Level] = JsonSerializer.SerializeToElement(headingLevel);
        props[PropNames.IsToggleable] = JsonSerializer.SerializeToElement(isToggleable); // Default false
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the HeadingBlockProps.
    /// </summary>
    /// <param name="headingLevel"></param>
    /// <param name="isToggleable"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with HeadingBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateHeadingBlockProps(int headingLevel = 1, bool isToggleable = false,
        string backgroundColor = PropValues.Default,
        string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddHeadingBlockProps(headingLevel, isToggleable)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    // List

    /// <summary>
    /// Adds NumberedListItemBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="number"></param>
    /// <returns>Dictionary with NumberedListItemBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddNumberedListItemBlockProps(this Dictionary<string, JsonElement> props, int? number = null)
    {
        switch (number)
        {
            case null:
                return props;
            case < 1:
                number = 1;
                break;
        }

        props[PropNames.Start] = JsonSerializer.SerializeToElement(number);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the NumberedListItemBlockProps.
    /// </summary>
    /// <param name="number"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with NumberedListItemBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateNumberedListItemBlockProps(int? number = null,
        string backgroundColor = PropValues.Default,
        string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddNumberedListItemBlockProps(number)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    /// <summary>
    /// Adds CheckedListItemBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="isChecked"></param>
    /// <returns>Dictionary with CheckedListItemBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddCheckedListItemBlockProps(this Dictionary<string, JsonElement> props, bool isChecked = false)
    {
        props[PropNames.Checked] = JsonSerializer.SerializeToElement(isChecked);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the CheckedListItemBlockProps.
    /// </summary>
    /// <param name="isChecked"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with CheckedListItemBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateCheckedListItemBlockProps(bool isChecked = false,
        string backgroundColor = PropValues.Default,
        string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddCheckedListItemBlockProps(isChecked)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    // Table

    /// <summary>
    /// Adds TableCellProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="colSpan"></param>
    /// <param name="rowSpan"></param>
    /// <returns>Dictionary with TableCellProps.</returns>
    public static Dictionary<string, JsonElement> AddTableCellProps(this Dictionary<string, JsonElement> props, int colSpan = 1, int rowSpan = 1)
    {
        props[PropNames.ColSpan] = JsonSerializer.SerializeToElement(colSpan);
        props[PropNames.RowSpan] = JsonSerializer.SerializeToElement(rowSpan);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the TableCellProps.
    /// </summary>
    /// <param name="colSpan"></param>
    /// <param name="rowSpan"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with TableCellProps.</returns>
    public static Dictionary<string, JsonElement> CreateTableCellProps(int colSpan = 1, int rowSpan = 1,
        string backgroundColor = PropValues.Default,
        string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddTableCellProps(colSpan, rowSpan)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    // Media

    /// <summary>
    /// Adds BaseEmbedBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <returns>Dictionary with BaseEmbedBlockProps.</returns>
    private static Dictionary<string, JsonElement> AddBaseEmbedBlockProps(this Dictionary<string, JsonElement> props, string url = "", string caption = "")
    {
        props[PropNames.Url] = JsonSerializer.SerializeToElement(url);
        props[PropNames.Caption] = JsonSerializer.SerializeToElement(caption);
        return props;
    }

    /// <summary>
    /// Adds the <see cref="PropNames.Name"/> prop to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="name"></param>
    /// <returns>Dictionary with <see cref="PropNames.Name"/> prop.</returns>
    private static Dictionary<string, JsonElement> AddNameProp(this Dictionary<string, JsonElement> props, string name = "")
    {
        props[PropNames.Name] = JsonSerializer.SerializeToElement(name);
        return props;
    }

    /// <summary>
    /// Adds FileBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <returns>Dictionary with FileBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddFileBlockProps(this Dictionary<string, JsonElement> props,
        string name = "", string url = "", string caption = "") =>
        props.AddNameProp(name).AddBaseEmbedBlockProps(url, caption);

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the FileBlockProps.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with FileBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateFileBlockProps(string name = "", string url = "", string caption = "",
        string backgroundColor = PropValues.Default,
        string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>().AddNameProp(name)
            .AddBaseEmbedBlockProps(url, caption)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    /// <summary>
    /// Adds ImageBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="previewWidth"></param>
    /// <returns>Dictionary with ImageBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddImageBlockProps(this Dictionary<string, JsonElement> props,
        string url = "", string caption = "", int previewWidth = PropValues.PreviewWidthWithDefault)
    {
        props.AddBaseEmbedBlockProps(url, caption);
        props[PropNames.PreviewWidth] = JsonSerializer.SerializeToElement(previewWidth);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the ImageBlock.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="previewWidth"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with ImageBlock.</returns>
    public static Dictionary<string, JsonElement> CreateImageBlock(string url = "", string caption = "",
        int previewWidth = PropValues.PreviewWidthWithDefault,
        string backgroundColor = PropValues.Default, string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddImageBlockProps(url, caption, previewWidth)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    /// <summary>
    /// Adds ImageBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="showPreview"></param>
    /// <param name="previewWidth"></param>
    /// <returns>Dictionary with ImageBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddVideoBlockProps(this Dictionary<string, JsonElement> props,
        string name = "", string url = "", string caption = "",
        bool showPreview = PropValues.ShowPreviewDefault, int? previewWidth = null)
    {
        props.AddNameProp(name).AddBaseEmbedBlockProps(url, caption);
        props[PropNames.ShowPreview] = JsonSerializer.SerializeToElement(showPreview);

        if (previewWidth is null) return props;

        props[PropNames.PreviewWidth] = JsonSerializer.SerializeToElement(previewWidth);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the VideoBlockProps.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="showPreview"></param>
    /// <param name="previewWidth"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with VideoBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateVideoBlockProps(string name = "", string url = "",
        string caption = "",
        bool showPreview = PropValues.ShowPreviewDefault, int? previewWidth = null,
        string backgroundColor = PropValues.Default, string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddVideoBlockProps(name, url, caption, showPreview, previewWidth)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    /// <summary>
    /// Adds AudioBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="showPreview"></param>
    /// <returns>Dictionary with AudioBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddAudioBlockProps(this Dictionary<string, JsonElement> props,
        string name = "", string url = "", string caption = "",
        bool showPreview = PropValues.ShowPreviewDefault)
    {
        props.AddNameProp(name).AddBaseEmbedBlockProps(url, caption);
        props[PropNames.ShowPreview] = JsonSerializer.SerializeToElement(showPreview);

        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the AudioBlockProps.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="caption"></param>
    /// <param name="showPreview"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with AudioBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateAudioBlockProps(string name = "", string url = "",
        string caption = "",
        bool showPreview = PropValues.ShowPreviewDefault,
        string backgroundColor = PropValues.Default, string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddAudioBlockProps(name, url, caption, showPreview)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    /// <summary>
    /// Adds CodeBlockProps to the given dictionary.
    /// </summary>
    /// <param name="props"></param>
    /// <param name="language"></param>
    /// <returns>Dictionary with CodeBlockProps.</returns>
    public static Dictionary<string, JsonElement> AddCodeBlockProps(this Dictionary<string, JsonElement> props, string language = PropValues.Text)
    {
        props[PropNames.Language] = JsonSerializer.SerializeToElement(language);
        return props;
    }

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the CodeBlockProps.
    /// </summary>
    /// <param name="language"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="textColor"></param>
    /// <param name="textAlignment"></param>
    /// <returns>Dictionary with CodeBlockProps.</returns>
    [Obsolete("Should not be used currently, since it appears that codeBlock in BlockNote doesn't contain default props.")]
    public static Dictionary<string, JsonElement> CreateCodeBlockPropsWithDefaults(string language = PropValues.Text,
        string backgroundColor = PropValues.Default, string textColor = PropValues.Default,
        string textAlignment = PropValues.Left) =>
        new Dictionary<string, JsonElement>()
            .AddCodeBlockProps(language)
            .AddDefaultProps(backgroundColor, textColor, textAlignment);

    /// <summary>
    /// Creates a dictionary with the default values and additional values of the CodeBlockProps.
    /// </summary>
    /// <param name="language"></param>
    /// <returns>Dictionary with CodeBlockProps.</returns>
    public static Dictionary<string, JsonElement> CreateCodeBlockProps(string language = PropValues.Text) =>
        new Dictionary<string, JsonElement>()
            .AddCodeBlockProps(language);

    // Divider (no props)

    // Todo: Add more methods for props of various Blocks
}
