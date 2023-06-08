namespace WebLogger.Render
{
    /// <summary>
    /// Describes the severity of the webLogger message.
    /// The primary use for this type is to render messages with colors and fonts.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Verbose message displayed in the Web UI
        /// </summary>
        Verbose,
        /// <summary>
        /// Information displayed in the Web UI
        /// </summary>
        Information,
        /// <summary>
        /// Warning messages displayed in the Web UI
        /// </summary>
        Warning,
        /// <summary>
        /// Errors and exceptions displayed in the Web UI
        /// </summary>
        Error
    }
}