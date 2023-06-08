namespace WebLogger
{
    /// <summary>
    /// Extension methods to register stored commands.
    /// </summary>
    public static class StoreCommandsInterfaceExtensions
    {
        /// <summary>
        /// Registers all the stored commands with the provided weblogger
        /// </summary>
        /// <param name="storedCommands">Command store</param>
        /// <param name="logger">logger to register commands.</param>
        /// <returns>same instance to chain the methods together</returns>
        public static IStoredCommands RegisterCommands(this IStoredCommands storedCommands, IWebLogger logger)
        {
            var commands = storedCommands.GetStoredCommands();

            foreach (var webLoggerCommand in commands)
            {
                logger.RegisterCommand(webLoggerCommand);
            }

            return storedCommands;
        }

        /// <summary>
        /// Registers all the commands in the provided command store.
        /// </summary>
        /// <param name="logger">logger to register commands.</param>
        /// <param name="storedCommands">Command store</param>
        /// <returns>same instance to chain the methods together</returns>
        public static IWebLogger RegisterCommandStore(this IWebLogger logger, IStoredCommands storedCommands)
        {
            var commands = storedCommands.GetStoredCommands();

            foreach (var webLoggerCommand in commands)
            {
                logger.RegisterCommand(webLoggerCommand);
            }

            return logger;
        }
    }
}