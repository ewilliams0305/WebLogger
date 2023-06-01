using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebLogger.Utilities
{
    /// <summary>
    /// Discovers commands
    /// </summary>
    internal sealed class CommandDiscovery
    {
        /// <summary>
        /// Discovers all the IWebLoggerCommand types defined in a provided assembly.
        /// <remarks>Commands must contain a parameter-less constructor for command instantiation.</remarks>
        /// </summary>
        /// <param name="assembly">Assembly to Scan</param>
        /// <returns>All valid commands in the assembly</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        public static IEnumerable<IWebLoggerCommand> DiscoveryAssemblyCommands(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var commands = new List<IWebLoggerCommand>();

            foreach (var type in assembly.GetTypes())
            {
                if(!typeof(IWebLoggerCommand).IsAssignableFrom(type))
                    continue;

                if (type.IsValueType)
                    continue;

                if (type.GetConstructor(Type.EmptyTypes) == null) 
                    continue;

                try
                {
                    var createdCommand = Activator.CreateInstance(type);
                    commands.Add(createdCommand as IWebLoggerCommand);
                }
                catch (TypeLoadException)
                {
                    throw;
                }
            }

            return commands;
        }
    }
}