using System;
using System.Collections.Generic;

namespace WebLogger
{
    /// <summary>
    /// A Console Command Object
    /// </summary>
    public sealed class WebLoggerCommand : IWebLoggerCommand
    {
        /// <summary>
        /// Console Command to send
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Description of command
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Help summary explaining parameters
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Callback Action when Console Receives this Command
        /// </summary>
        public Action<string, List<string>> CommandHandler { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="command"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        public WebLoggerCommand(Action<string, List<string>> handler, string command, string description = default, string help = default)
        {
            Command = command;  
            Description = description;  
            Help = help;
            CommandHandler = handler;   
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="command"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        public WebLoggerCommand(IWebLoggerHandler handler, string command, string description = default, string help = default)
        {
            Command = command;  
            Description = description;  
            Help = help;
            CommandHandler = handler.HandleCommand;   
        }
    }
}
