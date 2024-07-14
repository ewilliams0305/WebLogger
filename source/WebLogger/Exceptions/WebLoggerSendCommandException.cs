using System;

namespace WebLogger.Exceptions
{
    /// <summary>
    /// Custom exception thrown when the weblogger fails to send a data to the client.
    /// </summary>
    internal class WebLoggerCommandException : Exception
    {
        public WebLoggerCommandException(string command)
        :base($"Failed sending command {command} to client")
        {

        }

        public WebLoggerCommandException(string command, Exception innerException)
            : base($"Failed sending command {command} to client", innerException)
        {
            
        }
    }
}
