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
        internal  const string HtmlRoot = "WebLogger.HTML";
        /// <summary>
        /// The final location to store the html files after they have been extracted.
        /// </summary>
        internal static string HtmlDirectory = "html/logger";
        /// <summary>
        /// The file path for the index.html file after its been extracted.
        /// </summary>
        internal static string HtmlIndex = "html/logger/index.html";
    }
}