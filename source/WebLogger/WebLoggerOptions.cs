using System.Collections.Generic;
using WebLogger.Render;

namespace WebLogger
{
    /// <summary>
    /// Provides the IWebLogger factory configured optional parameters.
    /// </summary>
    public sealed class WebLoggerOptions
    {
        /// <summary>
        /// Web socket tcp port used to transport traffic between the client and server
        /// </summary>
        public int WebSocketTcpPort { get; set; } = 54321;
        /// <summary>
        /// Whether or not the server is secured web sockets or not
        /// <remarks>Secured WebSockets are not yest fully implemented</remarks>
        /// </summary>
        public bool Secured { get; set; } = false;
        /// <summary>
        /// Location of the HTML files to be extracted to and served.
        /// </summary>
        public string DestinationWebpageDirectory { get; set; } = ConstantValues.DefaultHtmlDirectory;
        /// <summary>
        /// Used to define colors utilized through the application. 
        /// </summary>
        public IColorFactory Colors => ColorFactory.Instance; 
        /// <summary>
        /// Provides an initial collection of commands to register with the logger.
        /// </summary>
        public IEnumerable<IWebLoggerCommand> Commands { get; set; }
    }
}