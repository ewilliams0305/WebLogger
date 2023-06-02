using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace WebLogger.Crestron.Simpl
{
    /// <summary>
    /// Creates a new console command to use in a SIMPL windows program
    /// </summary>
    public sealed class SimplCommand
    {

        internal static ConcurrentDictionary<string, IWebLoggerCommand> SimplCommands = new ConcurrentDictionary<string, IWebLoggerCommand>();

        private IWebLoggerCommand _cmd;

        /// <summary>
        /// Occurs when this command is received.
        /// </summary>
        public event EventHandler<SimplCommandArgs> CommandEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplCommand"/> class.
        /// </summary>
        public SimplCommand()
        {
        }

        /// <summary>
        /// Initializes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="description">The description.</param>
        /// <param name="help"></param>
        public void Initialize(string command, string description, string help)
        {
            _cmd = new WebLoggerCommand(CommandHandler, command, description, help);
            SimplCommands.TryAdd(_cmd.Command, _cmd);
        }

        /// <summary>
        /// Responds to console.
        /// </summary>
        /// <param name="message">The message.</param>
        public void RespondToConsole(string message)
        {
            LazyWebLogger.Instance.Write(message);
        }
        
        private ICommandResponse CommandHandler(string cmd, List<string> args)
        {
            var builder = new StringBuilder("");
            
            if (args != null && args.Count > 0)
                foreach (var item in args)
                    builder.Append(item).Append(" ");

            CommandEvent?.Invoke(this, new SimplCommandArgs() { RxData = builder.ToString() });

            return CommandResponse.Success(cmd, "Received By SIMPL Windows");
        }
    }
}
