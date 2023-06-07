using System.Drawing;

namespace WebLogger.Display
{
    /// <summary>
    /// Provides options to configure attributes on an HTML element.
    /// </summary>
    public sealed class HtmlElementOptions
    {
        /// <summary>
        /// Color applied to the elements Tag
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Font applied the html element.
        /// </summary>
        public string Font { get; }
        /// <summary>
        /// Style applied to the elements.
        /// </summary>
        public string Style { get;}

        /// <summary>
        /// Creates a new HTML Options object with a defined color, font, and style
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="font">Font Attribute</param>
        /// <param name="style">Style Attribute</param>
        public HtmlElementOptions(Color color = default, string font = default, string style = default)
        {
            Color = color;
            Font = font;
            Style = style;
        }
    }
}