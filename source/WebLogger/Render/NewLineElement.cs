using System.Text;

namespace WebLogger.Render
{
    /// <summary>
    /// Creates a New Line BR
    /// </summary>
    public readonly ref struct NewLineElement
    {
        private StringBuilder Builder => new StringBuilder(HtmlConstants.NewLine);

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
}