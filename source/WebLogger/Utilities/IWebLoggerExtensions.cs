using System.Reflection;

namespace WebLogger.Utilities
{
    /// <summary>
    /// Extensions designed to enhance the web logger developer experience
    /// </summary>
    public static class IWebLoggerExtensions
    {
        /// <summary>
        /// Discovers all the IWebLoggerCommands in an assembly.  Commands must be defined with a parameter-less constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IWebLogger DiscoverCommands(this IWebLogger logger, Assembly assembly)
        {
            var commands = CommandDiscovery.DiscoveryAssemblyCommands(assembly);

            foreach (var webLoggerCommand in commands)
            {
                logger.RegisterCommand(webLoggerCommand);
            }

            return logger;
        }
        
        /// <summary>
        /// Discovers all the provided commands that have been created in the WebLogger library
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IWebLogger DiscoverProvidedCommands(this IWebLogger logger)
        {
            var commands = CommandDiscovery
                .DiscoveryAssemblyCommands(
                    Assembly.GetAssembly(typeof(IAssemblyMarker)));

            foreach (var webLoggerCommand in commands)
            {
                logger.RegisterCommand(webLoggerCommand);
            }

            return logger;
        }
    }
}