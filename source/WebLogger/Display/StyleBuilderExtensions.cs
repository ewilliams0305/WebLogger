using System.Drawing;
using System.Text;

namespace WebLogger.Display
{
    internal static class StyleBuilderExtensions
    {
        internal static StringBuilder RenderStyleAttribute(this StringBuilder builder, HtmlElementOptions options)
        {
            return builder.Append(HtmlAttributes.Style)
                .Append("=\"")
                .AppendColorStyle(options.Color)
                .AppendFontFamily(options.FontFamily)
                .AppendFontSize(options.FontSize)
                .Append("\"");
        }

        internal static StringBuilder AppendColorStyle(this StringBuilder builder, Color color)
        {
            if(color.IsEmpty)
                return builder;

            return builder.Append(HtmlAttributes.Color)
                .Append(":")
                .RenderColor(color)
                .Append(";");
        }

        internal static StringBuilder AppendFontSize(this StringBuilder builder, int? pixels)
        {
            if (pixels == null || pixels == 0)
                return builder;

            return builder.Append(HtmlAttributes.FontSize)
                .Append(":")
                .Append(pixels)
                .Append("px")
                .Append(";");
        }
        internal static StringBuilder AppendFontFamily(this StringBuilder builder, string fontFamily)
        {
            if (string.IsNullOrEmpty(fontFamily))
                return builder;

            return builder.Append(HtmlAttributes.FontFamily)
                .Append(":")
                .Append(fontFamily)
                .Append(";");
        }

        internal static StringBuilder RenderColor(this StringBuilder builder, Color color)
        {
            if (color.IsEmpty)
                return builder;

            return builder.Append("#")
                .Append(color.R.ToString("X2"))
                .Append(color.G.ToString("X2"))
                .Append(color.B.ToString("X2"));
        }
    }
}