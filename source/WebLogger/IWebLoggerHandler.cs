using System.Collections.Generic;

namespace WebLogger
{
    /// <summary>
    /// Interface used to handle a received console command.
    /// </summary>
    public interface IWebLoggerHandler
    {
        /// <summary>
        /// The command handle that will be executed when the command is parsed from the weblogger
        /// </summary>
        /// <param name="command">Name of the command being handled.</param>
        /// <param name="args">Collected args received from the command.</param>
        /// <returns>A response to the weblogger cli</returns>
        string HandleCommand(string  command, List<string> args);
    }
}