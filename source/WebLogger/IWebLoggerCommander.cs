namespace WebLogger
{
    /// <summary>
    /// Implements the actions on the registered console commands.
    /// </summary>
    public interface IWebLoggerCommander
    {
        /// <summary>
        /// Register a new console command with this instance of the web logger
        /// </summary>
        /// <param name="command">The new console command to use with this instance of the web logger</param>
        /// <returns>Pass / Fail</returns>
        bool RegisterCommand(IWebLoggerCommand command);

        /// <summary>
        /// Removes the command from the web logger
        /// Once removed the command can no longer be invoked.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool RemoveCommand(IWebLoggerCommand command);

        /// <summary>
        /// Removes the command with the matching command name property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveCommand(string name);

        /// <summary>
        /// Help Console command Handler
        /// Prints all Registered commands to the console
        /// </summary>
        void ListCommands();

        /// <summary>
        /// Returns the help information pertaining to the specified command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string GetHelpInfo(string command);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool ExecuteCommand(string command);
    }
}