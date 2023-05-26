using System;

namespace VirtualControl.Debugging.WebLogger
{
    /// <summary>
    /// Creates a new Serilog Sink
    /// </summary>
    public static class WebloggerSinkExtensions
    {
        /// <summary>
        /// Provides an extension method to register the sink with the serilog configuration.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A Weblogger SINK</returns>
        public static LoggerConfiguration WebloggerSink(
            this LoggerSinkConfiguration loggerConfiguration,
            WebLogger logger,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new WebloggerSink(formatProvider, logger));
        }
    }
}