namespace WebLogger
{
    /// <summary>
    /// Defines all magic strings used by the application.
    /// </summary>
    internal static class ConstantValues
    {
        /// <summary>
        /// DLL path of the embedded resources
        /// </summary>
        internal const string HtmlRoot = "WebLogger.HTML";
        /// <summary>
        /// The file path for the index.html file after its been extracted.
        /// </summary>
        internal const string HtmlIndex = "index.html";
        /// <summary>
        /// Html information file.
        /// </summary>
        internal const string HtmlInfo = "info.txt";
        /// <summary>
        /// Default director to extract the embedded html directory
        /// </summary>
        internal const string DefaultHtmlDirectory = "C://Temp/WebLogger/Logger";
    }
}