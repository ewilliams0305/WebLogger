using System;
using Serilog.Events;

namespace WebLogger
{
    /// <summary>
    /// Renders a message to the weblogger
    /// </summary>
    public interface IRenderMessages
    {
        /// <summary>
        /// renders verbose messages
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string RenderVerbose(LogEvent logEvent, IFormatProvider formatProvider);
        /// <summary>
        /// renders an Information messages
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string RenderInformation(LogEvent logEvent, IFormatProvider formatProvider);
        /// <summary>
        /// renders warnings
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string RenderWarning(LogEvent logEvent, IFormatProvider formatProvider);
        /// <summary>
        /// renders error messages
        /// </summary>
        /// <param name="logEvent"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string RenderError(LogEvent logEvent, IFormatProvider formatProvider);
    }
}