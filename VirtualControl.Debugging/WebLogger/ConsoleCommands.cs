using System;
using System.Collections.Generic;

namespace VirtualControl.Debugging.WebLogger
{
    /// <summary>
    /// Stores custom console commands that will trigger custom methods.
    /// Console commands should follow the standard Crestron Format 
    /// [command][SPACE][PARAMS][SPACE etc...]
    /// </summary>
    public class ConsoleCommands
    {
        /// <summary>
        /// Stored commands
        /// </summary>
        private static Dictionary<string, ConsoleCommand> _consoleCmds = new Dictionary<string, ConsoleCommand>();

        /// <summary>
        /// Adds a new console command and stores a pointer to your callback method
        /// </summary>
        /// <param name="command">Conosle Command Containing Callback Action</param>
        public static void RegisterCommand(ConsoleCommand command)
        {
            var key = command.Command.ToUpper();

            if (_consoleCmds.ContainsKey(key))
                return;

            _consoleCmds.Add(key, command);
        }

        /// <summary>
        /// Searches for the string received and exuctes the Action<string> if found
        /// </summary>
        /// <param name="command">Fully formatted Command</param>
        /// <returns></returns>
        internal static bool CallCommand(string command)
        {
            var parts = command.Split(' ');
            var key = parts[0].ToUpper();
            
            List<string> args = new List<string>();

            if (parts.Length > 1)
                for(int i = 1; i < parts.Length; i++)
                    args.Add(parts[i]);

            if (_consoleCmds.ContainsKey(key))
            {
                var consoleCommand = _consoleCmds[key];

                var action = consoleCommand.CommandAction;

                if (action != null)
                    action(consoleCommand.Command, args);

                return true;
            }
            return false;
        }
        /// <summary>
        /// Searches for the Desired command and returns the Help Information
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static string GetHelpInfo(string command)
        {
            var parts = command.Split(' ');
            var key = parts[0].ToUpper().Replace("?", "");

            if (_consoleCmds.ContainsKey(key))
            {
                var consoleCommand = _consoleCmds[key];

                return consoleCommand.Command + "|" + consoleCommand.Description + "|" + consoleCommand.Help;
            }
            return "VC4> UNKNOWN COMMAND";
        }
        /// <summary>
        /// Returns a list of all commands and discriptions
        /// </summary>
        /// <returns>Formatted list of commands</returns>
        internal static List<string> GetAllCommands()
        {
            var commands = new List<string>();

            foreach (var cmd in _consoleCmds.Values)
            {
                commands.Add(cmd.Command + "|" + cmd.Description);
            }

            return commands;
        }
    }
}
