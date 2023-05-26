using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebLogger
{
    /// <summary>
    /// Interface used to handle a received console command.
    /// </summary>
    public interface IAsyncWebLoggerHandler
    {
        /// <summary>
        /// The command handle that will be executed when the command is parsed from the weblogger
        /// </summary>
        /// <param name="command">Name of the command being handled.</param>
        /// <param name="args">Collected args received from the command.</param>
        Task HandleCommand(string  command, List<string> args);
    }
}