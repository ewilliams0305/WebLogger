using Serilog;
using Serilog.Configuration;
using System;

namespace WebLogger
{

    /// <summary>
    /// Creates a new Serilog Sink
    /// </summary>
    public static class WebloggerSinkExtensions
    {
        /// <summary>
        /// Provides an extension method to register the sink with the serilog configuration.
        /// </summary>
        /// <param name="loggerConfiguration">This logger configuration.</param>
        /// <param name="options">WebLogger factory options configures the weblogger</param>
        /// <param name="logger">Provides access to the logger after the sink has constructed the logger</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="renderer">Optional rendering used to format the serilog outputs</param>
        /// <returns></returns>
        public static LoggerConfiguration WebloggerSink(
            this LoggerSinkConfiguration loggerConfiguration,
            Action<WebLoggerOptions> options,
            Action<IWebLogger> logger,
            IFormatProvider formatProvider = null,
            IRenderMessages renderer = null)
        {
            var sink = new WebLoggerSink(options, logger, formatProvider, renderer);
            return loggerConfiguration.Sink(sink);
        }
    }
}