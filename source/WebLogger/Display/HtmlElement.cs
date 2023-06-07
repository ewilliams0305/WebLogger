using System;
using System.Text;

namespace WebLogger.Display
{
    /// <summary>
    /// A fully formatted HTML Element
    /// </summary>
    public readonly ref struct HtmlElement
    {
        private StringBuilder Builder { get; }

        public int HeaderLength { get; }
        public Element Element { get; }

        /// <summary>
        /// Creates a new fully rendered HTML element
        /// </summary>
        /// <param name="element">The Type of Element to Render</param>
        /// <param name="value">Inner element data</param>
        /// <param name="options">Formatting attribute options</param>
        public HtmlElement(Element element, string value = default, HtmlElementOptions options = default)
        {
            Element = element;
            Builder = Render(element, value, options).Item1;
            HeaderLength = Render(element, value, options).Item2;
        }

        /// <summary>
        /// Creates a new HTML element with an inner element.
        /// </summary>
        /// <param name="outerElement">The outer element to wrap around the inner element.</param>
        /// <param name="innerElement">The inside of the outer element.</param>
        public HtmlElement(HtmlElement outerElement, HtmlElement innerElement)
        {
            Element = outerElement.Element;
            Builder = RenderInnerElement(outerElement, innerElement).Item1;
            HeaderLength = RenderInnerElement(outerElement, innerElement).Item2;
        }

        private static (StringBuilder, int) Render(Element element, string value, HtmlElementOptions options = default)
        {
            var builder = new StringBuilder();

            builder.AppendOpener(element, options);

            var headerLength = builder.Length;

            builder.Append(string.IsNullOrEmpty(value) ? "" : value)
                .AppendCloser(element);

            return (builder, headerLength);
        }
        
        private static (StringBuilder, int) RenderInnerElement(HtmlElement outerElement, HtmlElement innerElement)
        {
            var length = outerElement.HeaderLength + innerElement.HeaderLength;
            var builder = new StringBuilder().Append(outerElement.Builder.Insert(outerElement.HeaderLength, innerElement));
            return (builder, length);
        }

        /// <summary>
        /// Converts the html element to a fully formatted string.
        /// </summary>
        /// <param name="element">The element to convert</param>
        public static implicit operator string(HtmlElement element) => element.Builder.ToString();

        /// <summary>
        /// Converts the html element to a string builder for additional formatting.
        /// </summary>
        /// <param name="element"></param>
        public static implicit operator StringBuilder(HtmlElement element) => element.Builder;
    }
}
