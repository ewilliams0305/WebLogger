using System.Reflection;
using WebLogger.Utilities;

namespace WebLogger.Crestron
{
    /// <summary>
    /// Extension methods used to configure and enhance the weblogger experience.
    /// </summary>
    public static class WebLoggerExtensions
    {
        /// <summary>
        /// Using the properties defined by the <see cref="IWebLogger"/> configuration a HTTP server will be created
        /// at the specified port to serve the HTML files located in the HTML directory.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpSeverPort"></param>
        /// <returns></returns>
        public static IWebLogger ServeWebLoggerHtml(this IWebLogger logger, int httpSeverPort)
        {
            var webPageServer = new WebLoggerHttpServer(
                port: httpSeverPort,
                directory: logger.HtmlDirectory);

            return logger;
        }

        /// <summary>
        /// Discovers all the commands in the WebLogger.Crestron Assembly.
        /// </summary>
        /// <param name="logger">The logger to add the discovered commands</param>
        /// <returns>the same instance of the logger so methods can be chained</returns>
        public static IWebLogger DiscoverCrestronCommands(this IWebLogger logger)
        {
            logger.DiscoverCommands(Assembly.GetAssembly(typeof(IAssemblyMarker)));

            return logger;
        }
    }
}