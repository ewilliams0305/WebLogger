using System;
using WebLogger.Utilities;

namespace WebLogger
{
    /// <summary>
    /// Creates instances of the IWebLogger interface.
    /// </summary>
    public static class WebLoggerFactory
    {
        /// <summary>
        /// Creates a new instance of the default implementation of the IWebLogger using the internal class WebLogger
        /// </summary>
        /// <param name="options">Override the default weblogger behavior and provide optional configuration parameters.</param>
        /// <returns>IWebLogger instance</returns>
        public static IWebLogger CreateWebLogger(Action<WebLoggerOptions> options = null)
        {
            var configuration = new WebLoggerOptions();
            options?.Invoke(configuration);

            var logger = new WebLogger(
                configuration.WebSocketTcpPort,
                configuration.Secured,
                configuration.DestinationWebpageDirectory);

            logger.RegisterCommands(configuration.Commands);

            return logger;
        }
    }
}