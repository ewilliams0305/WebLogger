using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualControl.Debugging.WebLogger
{
    /// <summary>
    /// Console command callback
    /// </summary>
    /// <param name="command">Command that was issued</param>
    /// <param name="arguments">Argument passed into the command seperated by spaces</param>
    public delegate void ConsoleCommandCallback(string command, List<string> arguments);

    /// <summary>
    /// A Console Command Object
    /// </summary>
    public class ConsoleCommand
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
        /// <summary>
        /// Callback Action when Conosle Receives this Command
        /// </summary>
        public ConsoleCommandCallback CommandAction { get; set; }
    }
}
