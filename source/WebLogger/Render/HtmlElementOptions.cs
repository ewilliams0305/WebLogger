﻿using System.Drawing;

namespace WebLogger.Render
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
        public string FontFamily { get; }
        /// <summary>
        /// Determines the size of the font in pixels.
        /// </summary>
        public int? FontSize { get; set; }
        /// <summary>
        /// Provides the option to embedded style form-mated strings.
        /// i.e. to set the background color enter 'background-color:#FF0000;'
        /// </summary>
        public string AdditionalStyles { get; }

        /// <summary>
        /// Creates a new HTML Options object with a defined color, font, and style
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="fontFamily">Font Attribute</param>
        /// <param name="fontSize">Style Attribute</param>
        public HtmlElementOptions(Color color = default, string fontFamily = default, int? fontSize = null, string additionalStyles = null)
        {
            Color = color;
            FontFamily = fontFamily;
            FontSize = fontSize;
            AdditionalStyles = additionalStyles;
        }

        public static implicit operator HtmlElementOptions(Color color) => new HtmlElementOptions(color);
        public static implicit operator Color(HtmlElementOptions options) => options.Color;
    }
}