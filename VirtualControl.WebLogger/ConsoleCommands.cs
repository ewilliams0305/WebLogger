using System.Collections.Generic;
using System.Linq;

namespace VirtualControl.WebLogger
{
    /// <summary>
    /// Stores custom console commands that will trigger custom methods.
    /// [command][SPACE][PARAMS][SPACE etc...]
    /// </summary>
    public sealed class ConsoleCommands
    {
        /// <summary>
        /// Stored commands
        /// </summary>
        private static readonly Dictionary<string, ConsoleCommand> ConsoleCommandStore = new Dictionary<string, ConsoleCommand>();

        /// <summary>
        /// Adds a new console command and stores a pointer to your callback method
        /// </summary>
        /// <param name="command">Console Command Containing Callback Action</param>
        public static void RegisterCommand(ConsoleCommand command)
        {
            var key = command.Command.ToUpper();

            if (ConsoleCommandStore.ContainsKey(key))
                return;

            ConsoleCommandStore.Add(key, command);
        }

        /// <summary>
        /// Searches for the string received and executes the if found
        /// </summary>
        /// <param name="command">Fully formatted Command</param>
        /// <returns></returns>
        internal static bool CallCommand(string command)
        {
            var parts = command.Split(' ');
            var key = parts[0].ToUpper();
            
            var args = new List<string>();

            if (parts.Length > 1)
                for(var i = 1; i < parts.Length; i++)
                    args.Add(parts[i]);

            if (!ConsoleCommandStore.ContainsKey(key)) 
                return false;

            var consoleCommand = ConsoleCommandStore[key];
            consoleCommand.CommandHandler?.Invoke(command, args);

            return true;
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

            if (!ConsoleCommandStore.ContainsKey(key)) 
                return "VC4> UNKNOWN COMMAND";

            var consoleCommand = ConsoleCommandStore[key];
            return consoleCommand.Command + "|" + consoleCommand.Description + "|" + consoleCommand.Help;
        }
        /// <summary>
        /// Returns a list of all commands and descriptions
        /// </summary>
        /// <returns>Formatted list of commands</returns>
        internal static List<string> GetAllCommands()
        {
            return ConsoleCommandStore.Values
                .Select(cmd => cmd.Command + "|" + cmd.Description)
                .ToList();
        }
    }
}
