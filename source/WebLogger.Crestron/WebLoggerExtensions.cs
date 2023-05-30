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
    }
}