using System.Reflection;
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
            
            return builder.RenderStyleAttribute(options);
        }
    }
}