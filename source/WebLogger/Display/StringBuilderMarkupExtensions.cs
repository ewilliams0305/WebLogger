using System.Drawing;
using System.Text;

namespace WebLogger.Display
{
    internal static class StringBuilderMarkupExtensions
    {
        public static StringBuilder AppendOpener(this StringBuilder builder, Element element, HtmlElementOptions options = default)
        {
            return builder.Append(HtmlElements.Open)
                .Append(HtmlElements.GetElement(element))
                .Append(options != null ? " " : string.Empty)
                .RenderAttributes(options)
                .Append(HtmlElements.Close);
        }

        public static StringBuilder AppendCloser(this StringBuilder builder, Element element)
        {
            return builder.Append(HtmlElements.ClosureHeader)
                .Append(HtmlElements.GetElement(element))
                .Append(HtmlElements.Close);
        }

        public static StringBuilder RenderAttributes(this StringBuilder builder, HtmlElementOptions options = default)
        {
            if (options == null)
            {
                return builder;
            }

            if(!options.Color.IsEmpty)
                builder.Append(HtmlAttributes.Color)
                    .Append("=\"")
                    .RenderColor(options.Color)
                    .Append(string.IsNullOrWhiteSpace(options.Font) && string.IsNullOrWhiteSpace(options.Style) ? "\"" : "\" ");

            if (!string.IsNullOrWhiteSpace(options.Font))
                builder.Append(HtmlAttributes.Font)
                    .Append("=\"")
                    .Append(options.Font)
                    .Append(string.IsNullOrWhiteSpace(options.Style) ? "\"" : "\" ");

            if (!string.IsNullOrWhiteSpace(options.Style))
                builder.Append(HtmlAttributes.Style)
                    .Append("=\"")
                    .Append(options.Style)
                    .Append("\"");

            return builder;
        }

        public static StringBuilder RenderColor(this StringBuilder builder, Color color)
        {
            if(color == default)
                return builder;

            return builder.Append("#")
                .Append(color.R.ToString("X2"))
                .Append(color.G.ToString("X2"))
                .Append(color.B.ToString("X2"));
        }
    }
}