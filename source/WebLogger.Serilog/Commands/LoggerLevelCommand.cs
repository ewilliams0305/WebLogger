using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace WebLogger
{
    /// <summary>
    /// Sets the serilog log level
    /// </summary>
    public class LoggerLevelCommand : IWebLoggerCommand
    {

        /// <summary>
        /// Log level switcher
        /// </summary>
        public LoggingLevelSwitch LoggingLevelSwitch;

        /// <summary>
        /// Creates the new command
        /// </summary>
        /// <param name="level">with a provided level</param>
        public LoggerLevelCommand(LogEventLevel level)
        {
            LoggingLevelSwitch = new LoggingLevelSwitch(level);
        }

        /// <inheritdoc />
        public string Command => "SERILOGLEVEL";
        /// <inheritdoc />
        public string Description => "Changes the Verbosity";
        /// <inheritdoc />
        public string Help => "SerilogLevel [LoggingLevel] (Values: Debug, Verbose, Information, Warning, Error, Fatal)";
        /// <inheritdoc />
        public Func<string, List<string>, ICommandResponse> CommandHandler => Handler;

        /// <summary>
        /// Handles the data parameters passed from the command.
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="args">The argument list</param>
        /// <returns></returns>
        public ICommandResponse Handler(string command, List<string> args)
        {
            if (args == null || args.Count == 0)
            {
                return CommandResponse.Failure(this,
                    "Please provide logging level argument (Debug, Verbose, Information, Warning, Error, Fatal)");
            }

            if(!Enum.TryParse(args[0], true, out LogEventLevel level))
            {
                return CommandResponse.Failure(this,
                    "Invalid logging level argument (Debug, Verbose, Information, Warning, Error, Fatal)");
            }

            LoggingLevelSwitch.MinimumLevel = level;
            return CommandResponse.Success(this, $"Changed Logging Level to {level}");
        }
    }
}
