using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WebLogger
{
    /// <summary>
    /// Stores custom console commands that will trigger custom methods.
    /// [command][SPACE][PARAMS][SPACE etc...]
    /// </summary>
    internal sealed class WebLoggerCommands : IDisposable
    {
        /// <summary>
        /// Stored commands
        /// </summary>
        private readonly ConcurrentDictionary<string, IWebLoggerCommand> _commandStore = new ConcurrentDictionary<string, IWebLoggerCommand>();

        /// <summary>
        /// Adds a new console command and stores a pointer to your callback method
        /// </summary>
        /// <param name="command">Console Command Containing Callback Action</param>
        internal bool RegisterCommand(IWebLoggerCommand command)
        {
            var key = command.Command.ToUpper();
            return _commandStore.TryAdd(key, command);
        }
        
        /// <summary>
        /// Adds a new console command and stores a pointer to your callback method
        /// </summary>
        /// <param name="command">Console Command Containing Callback Action</param>
        internal bool RemoveCommand(IWebLoggerCommand command)
        {
            var key = command.Command.ToUpper();
            return _commandStore.TryRemove(key, out _);
        }
        
        /// <summary>
        /// Adds a new console command and stores a pointer to your callback method
        /// </summary>
        /// <param name="command">Console Command Containing Callback Action</param>
        internal bool RemoveCommand(string command)
        {
            var key = command.ToUpper();
            return _commandStore.TryRemove(key, out _);
        }

        /// <summary>
        /// Searches for the string received and executes the if found
        /// </summary>
        /// <remarks>If the execute command methods returns valid data and was successful the method will return true and out results</remarks>
        /// <param name="command">Fully formatted Command</param>
        /// <returns></returns>
        internal ICommandResponse ExecuteCommand(string command)
        {
            var parts = command.Split(' ');
            var key = parts[0].ToUpper();

            var commandValue = _commandStore.Values
                .FirstOrDefault(c => c.Command.StartsWith(key));

            if (commandValue == null)
            {
                return CommandResponse.Error(command, "Not Registered");
            }

            var args = new List<string>();

            if (parts.Length > 1)
                for(var i = 1; i < parts.Length; i++)
                    args.Add(parts[i]);

            // var consoleCommand = _commandStore[key];
            return commandValue.CommandHandler.Invoke(command, args);
        }
        /// <summary>
        /// Searches for the Desired command and returns the Help Information
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal string GetHelpInfo(string command)
        {
            var parts = command.Split(' ');
            var key = parts[0].ToUpper().Replace("?", "");

            if (!_commandStore.ContainsKey(key)) 
                return "WEB LOGGER> UNKNOWN COMMAND";

            var consoleCommand = _commandStore[key];
            return consoleCommand.Command + "|" + consoleCommand.Description + "|" + consoleCommand.Help;
        }
        /// <summary>
        /// Returns a list of all commands and descriptions
        /// </summary>
        /// <returns>Formatted list of commands</returns>
        internal IEnumerable<IWebLoggerCommand> GetAllCommands()
        {
            return _commandStore.Values;
        }


        private void ReleaseUnmanagedResources()
        {
            _commandStore.Clear();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WebLoggerCommands()
        {
            ReleaseUnmanagedResources();
        }
    }
}
