using System.Text;

namespace WebLogger.Render
{
    internal static class StringBuilderMarkupExtensions
    {
        public static StringBuilder AppendOpener(this StringBuilder builder, Element element, HtmlElementOptions options = default)
        {
            return builder.Append(HtmlConstants.Open)
                .Append(HtmlConstants.GetElement(element))
                .Append(options != null ? " " : string.Empty)
                .RenderAttributes(options)
                .Append(HtmlConstants.Close);
        }

        public static StringBuilder AppendCloser(this StringBuilder builder, Element element)
        {
            return builder.Append(HtmlConstants.ClosureHeader)
                .Append(HtmlConstants.GetElement(element))
                .Append(HtmlConstants.Close);
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