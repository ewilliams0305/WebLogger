using System.Text;

namespace WebLogger.Render
{
    public readonly ref struct NewLineElement
    {
        private StringBuilder Builder => new StringBuilder(HtmlElements.NewLine);

        ///// <summary>
        ///// Appends a string to the html element
        ///// </summary>
        ///// <param name="value">adds the string value to the html element.</param>
        ///// <returns>this</returns>
        //public NewLineElement Append(string value)
        //{
        //    Builder.Append(value);
        //    return this;
        //}

        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public NewLineElement Append(StringBuilder value)
        {
            Builder.Append(value);
            return this;
        }

        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public NewLineElement Append(HtmlElement value)
        {
            Builder.Append(value);
            return this;
        }

        /// <summary>
        /// Converts the html element to a string builder for additional formatting.
        /// </summary>
        /// <param name="element"></param>
        public static implicit operator string(NewLineElement element) => element.Builder.ToString();
        /// <summary>
        /// Converts the html element to a string builder for additional formatting.
        /// </summary>
        /// <param name="element"></param>
        public static implicit operator StringBuilder(NewLineElement element) => element.Builder;
    }
    
    public readonly ref struct ClosureElement
    {
        private StringBuilder Builder => new StringBuilder(HtmlElements.Closure);

        ///// <summary>
        ///// Appends a string to the html element
        ///// </summary>
        ///// <param name="value">adds the string value to the html element.</param>
        ///// <returns>this</returns>
        //public ClosureElement Append(string value)
        //{
        //    Builder.Append(value);
        //    return this;
        //}

        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public ClosureElement Append(StringBuilder value)
        {
            Builder.Append(value);
            return this;
        }

        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public ClosureElement Append(HtmlElement value)
        {
            Builder.Append(value);
            return this;
        }

        /// <summary>
        /// Converts the html element to a string builder for additional formatting.
        /// </summary>
        /// <param name="element"></param>
        public static implicit operator StringBuilder(ClosureElement element) => element.Builder;
    }

    /// <summary>
    /// A fully formatted HTML Element
    /// </summary>
    public readonly ref struct HtmlElement
    {
        #region static Element Factories

        public static NewLineElement NewLine => new NewLineElement();

        public static ClosureElement Closure => new ClosureElement();

        public static HtmlElement SpanElement(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Span, innerText, options);
        }

        public static HtmlElement DivElement(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Div, innerText, options);
        }

        public static HtmlElement ParagraphElement(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Paragraph, innerText, options);
        }

        public static HtmlElement ParagraphElement(HtmlElement innerElement = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Paragraph, innerElement, options);
        }

        public static HtmlElement HeaderElement(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Header, innerText, options);
        }

        public static HtmlElement H1Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H1, innerText, options);
        }

        public static HtmlElement H2Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H2, innerText, options);
        }

        public static HtmlElement H3Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H3, innerText, options);
        }

        public static HtmlElement H4Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H4, innerText, options);
        }

        public static HtmlElement H5Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H5, innerText, options);
        }

        #endregion

        private StringBuilder Builder { get; }

        /// <summary>
        /// Index of the first closing header html tag >
        /// </summary>
        public int HeaderLength { get; }
        /// <summary>
        /// Type of element
        /// </summary>
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
            var result = Render(element, value, options);
            Builder = result.Item1;
            HeaderLength = result.Item2;
        }

        /// <summary>
        /// Creates a new fully rendered HTML element
        /// </summary>
        /// <param name="element">The Type of Element to Render</param>
        /// <param name="builder">Inner element data</param>
        /// <param name="options">Formatting attribute options</param>
        public HtmlElement(Element element, StringBuilder builder, HtmlElementOptions options = default)
        {
            Element = element;
            var result = Render(element, builder, options);
            Builder = result.Item1;
            HeaderLength = result.Item2;
        }

        /// <summary>
        /// Creates a new HTML element with an inner element.
        /// </summary>
        /// <param name="outerElement">The outer element to wrap around the inner element.</param>
        /// <param name="innerElement">The inside of the outer element.</param>
        public HtmlElement(HtmlElement outerElement, HtmlElement innerElement)
        {
            Element = outerElement.Element;

            var result = RenderInnerElement(outerElement, innerElement);
            Builder = result.Item1;
            HeaderLength = result.Item2;
        }

        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public HtmlElement Append(string value)
        {
            Builder.Append(value);
            return this;
        }
        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public HtmlElement Append(StringBuilder value)
        {
            Builder.Append(value);
            return this;
        }

        /// <summary>
        /// Appends a builder to the html element
        /// </summary>
        /// <param name="value">adds the builders string value to the html element.</param>
        /// <returns>this</returns>
        public HtmlElement Append(HtmlElement value)
        {
            Builder.Append(value);
            return this;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HtmlElement AppendLine()
        {
            Builder.Append(HtmlElements.NewLine);
            return this;
        }

        /// <summary>
        /// Inserts a inner HTML element into this element.
        /// </summary>
        /// <param name="innerElement">Element to insert</param>
        /// <returns>This element.</returns>
        public HtmlElement Insert(HtmlElement innerElement)
        {
            var result = RenderInnerElement(this,  innerElement);
            return this;
        }
        
        private static (StringBuilder, int) Render(Element element, string value, HtmlElementOptions options = default)
        {
            var builder = new StringBuilder();

            builder.AppendOpener(element, options);
            var headerLength = builder.Length + (value?.Length ?? 0);

            builder.Append(string.IsNullOrEmpty(value) ? "" : value)
                .AppendCloser(element);

            return (builder, headerLength);
        }

        private static (StringBuilder, int) Render(Element element, StringBuilder value, HtmlElementOptions options = default)
        {
            var builder = new StringBuilder();

            builder.AppendOpener(element, options);
            var headerLength = builder.Length + (value?.Length ?? 0);

            builder.Append(value)
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
        /// Renders the HTML Element as a string literal
        /// </summary>
        /// <returns></returns>
        public string Render()
        {
            return Builder.ToString();
        }

        /// <summary>
        /// Converts the html element to a string builder for additional formatting.
        /// </summary>
        /// <param name="element"></param>
        public static implicit operator StringBuilder(HtmlElement element) => element.Builder;
    }
}
