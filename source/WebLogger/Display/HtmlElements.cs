using System.Collections.Generic;

namespace WebLogger.Display
{
    /// <summary>
    /// Provides the constant string definitions required to created an HTML Tag
    /// </summary>
    internal sealed class HtmlElements
    {
        private const string Paragraph = "p";
        private const string Header = "header";
        private const string H1 = "h1";
        private const string H2 = "h2";
        private const string H3 = "h3";
        private const string H4 = "h4";
        private const string H5 = "h5";
        private const string Div = "div";
        private const string Span = "span";

        public const string Open = "<";
        public const string Close = ">";
        public const string Closure = "</>";
        public const string ClosureHeader = "</";
        public const string NewLine = "<br>";

        private static readonly Dictionary<Element, string> Elements = new Dictionary<Element, string>
        {
            { Element.Header, Header },
            { Element.H1, H1 },
            { Element.H2, H2 },
            { Element.H3, H3 },
            { Element.H4, H4 },
            { Element.H5, H5 },
            { Element.Div, Div },
            { Element.Span, Span },
            { Element.Paragraph, Paragraph }
        };

        /// <summary>
        /// Returns the element text
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetElement(Element element)
        {
            return Elements.ContainsKey(element) 
                ? Elements[element] 
                : string.Empty;
        }
    }
}