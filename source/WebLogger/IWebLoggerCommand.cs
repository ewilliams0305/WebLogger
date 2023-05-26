using System.Collections.Generic;
using System;

namespace WebLogger
{
    /// <summary>
    /// Defines a console command executed by the web logger CLI
    /// </summary>
    public interface IWebLoggerCommand
    {
        /// <summary>
        /// Console Command to send
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Description of command
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Help summary explaining parameters
        /// </summary>
        string Help { get; }

        /// <summary>
        /// The callback function invoked when the console command is received. 
        /// </summary>
        Action<string, List<string>> CommandHandler { get; }
    }
}