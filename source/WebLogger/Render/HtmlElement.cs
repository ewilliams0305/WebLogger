using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLogger.Render
{
    /// <summary>
    /// A fully formatted HTML Element
    /// </summary>
    public readonly ref struct HtmlElement
    {
        #region static Element Factories

        #region BASE ELEMENTS

        /// <summary>
        /// Creates a SPAN
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Span(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Span, innerText, options);
        }
        /// <summary>
        /// Creates a SPAN with an inner HTML element.
        /// </summary>
        /// <param name="innerElement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Span(HtmlElement innerElement, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Span, innerElement, options);
        }
        /// <summary>
        /// Creates a DIV
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Div(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Div, innerText, options);
        }
        /// <summary>
        /// Creates a DIV with an inner element.
        /// </summary>
        /// <param name="innerElement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Div(HtmlElement innerElement, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Div, innerElement, options);
        }
        /// <summary>
        /// Creates a paragraph
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Paragraph(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Paragraph, innerText, options);
        }
        /// <summary>
        /// Creates a paragraph with an inner element
        /// </summary>
        /// <param name="innerElement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Paragraph(HtmlElement innerElement, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Paragraph, innerElement, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement HeaderElement(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Header, innerText, options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerElement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement HeaderElement(HtmlElement innerElement, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Header, innerElement, options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement H1Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H1, innerText, options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement H2Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H2, innerText, options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement H3Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H3, innerText, options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement H4Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H4, innerText, options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerText"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement H5Element(string innerText = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.H5, innerText, options);
        }

        #endregion

        #region TABLES

        /// <summary>
        /// Creates a multi-dimensional table
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="tableData"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Table(List<string> headers, List<List<string>> tableData, HtmlElementOptions options = default)
        {
            var tableHeader = TableHeader(headers, options);
            var table = HtmlElement.Table();

            foreach (var tableEntry in tableData)
            {
                var row = HtmlElement
                    .TableRow()
                    .Insert(TableRow(tableEntry));

                table.Insert(row);
            }

            return table.Insert(tableHeader);
        }

        /// <summary>
        /// Creates a new Table
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Table(HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Table, options: options);
        }
        /// <summary>
        /// Creates a table with inner elements.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement Table(HtmlElement rows, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.Table, rows, options);
        }

        /// <summary>
        /// Creates a Table Row.
        /// </summary>
        /// <param name="innerElement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement TableRow(HtmlElement innerElement = default, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.TableRow, innerElement, options);
        }
        /// <summary>
        /// Creates a Table Row with populated data elements.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement TableRow(List<string> columns, HtmlElementOptions options = default)
        {
            var tableHeader = TableRow(options: options);

            for (var i = columns.Count - 1; i >= 0; i--)
                tableHeader.Insert(TableData(columns[i]));

            return tableHeader;
        }
        /// <summary>
        /// Creates a data entry in a table.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement TableData(string value, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.TableData, value, options);
        }
        /// <summary>
        /// Creates a table header item
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement TableHeader(string value, HtmlElementOptions options = default)
        {
            return new HtmlElement(Element.TableHeader, value, options);
        }
        /// <summary>
        /// Creates a Table header with values.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static HtmlElement TableHeader(List<string> columns, HtmlElementOptions options = default)
        {
            var tableHeader = TableRow(options: options);

            for (var i = columns.Count - 1; i >= 0; i--)
                tableHeader.Insert(TableHeader(columns[i]));

            return tableHeader;
        }

        #endregion

        #endregion

        #region INSTANCE MEMBERS
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
            Builder.Append(HtmlConstants.NewLine);
            return this;
        }

        /// <summary>
        /// Inserts a inner HTML element into this element.
        /// </summary>
        /// <param name="innerElement">Element to insert</param>
        /// <returns>This element.</returns>
        public HtmlElement Insert(HtmlElement innerElement)
        {
            RenderInnerElement(this, innerElement);
            return this;
        }

        /// <summary>
        /// Adds an element before the provided element.
        /// <remarks>This is useful for building tables in loops.</remarks>
        /// </summary>
        /// <param name="beforeElement">Element to add before this element</param>
        /// <returns>This element.</returns>
        public HtmlElement Before(HtmlElement beforeElement)
        {
            RenderBeforeElement(this, beforeElement);
            return this;
        }

        #endregion

        #region RENDERS

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

        private static (StringBuilder, int) RenderBeforeElement(HtmlElement beforeElement, HtmlElement afterElement)
        {
            var length = beforeElement.HeaderLength;
            var builder = beforeElement.Builder.Insert(0, afterElement);
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

        #endregion

        #region OPERATORS

        /// <summary>
        /// Converts the html element to a string builder for additional formatting.
        /// </summary>
        /// <param name="element"></param>
        public static implicit operator StringBuilder(HtmlElement element) => element.Builder;

        #endregion

    }
}
