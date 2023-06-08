using System.Text;

namespace WebLogger.Render
{
    /// <summary>
    /// Creates a HTML tag closure
    /// </summary>
    public readonly ref struct ClosureElement
    {
        private StringBuilder Builder => new StringBuilder(HtmlConstants.Closure);

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
}