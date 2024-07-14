using System.Collections.Generic;

namespace WebLogger
{
    /// <summary>
    /// This auto generated interface provides one method to return the stored commands.
    /// </summary>
    public interface IStoredCommands
    {
        /// <summary>
        /// Returns all stored commands from the instance of the command store class.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IWebLoggerCommand> GetStoredCommands();
    }
}