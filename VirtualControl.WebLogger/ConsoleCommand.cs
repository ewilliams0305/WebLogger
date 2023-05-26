using System;
using System.Collections.Generic;

namespace VirtualControl.WebLogger
{
    /// <summary>
    /// A Console Command Object
    /// </summary>
    public sealed class ConsoleCommand
    {
        /// <summary>
        /// Console Command to send
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Description of command
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Help summary explaining parameters
        /// </summary>
        public string Help { get; set; }
        ///// <summary>
        ///// Callback Action when Console Receives this Command
        ///// </summary>
        public Action<string, List<string>> CommandHandler;

        /// <summary>
        /// Default constructor
        /// Creates an instance with undefined properties
        /// </summary>
        public ConsoleCommand()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        /// <param name="handler"></param>
        public ConsoleCommand(string command, string description, string help, Action<string, List<string>> handler)
        {
            Command = command;  
            Description = description;  
            Help = help;
            CommandHandler = handler;   
        }
    }
}
