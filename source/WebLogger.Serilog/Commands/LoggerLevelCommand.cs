using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace WebLogger
{
    public class LoggerLevelCommand : IWebLoggerCommand
    {
        public LoggingLevelSwitch LoggingLevelSwitch;

        public LoggerLevelCommand(LogEventLevel level)
        {
            LoggingLevelSwitch = new LoggingLevelSwitch(level);
        }

        public string Command => "SERILOGLEVEL";
        public string Description => "Changes the Verbosity";
        public string Help => "SerilogLevel [LoggingLevel] (Values: Debug, Verbose, Information, Warning, Error, Fatal)";
        public Func<string, List<string>, ICommandResponse> CommandHandler => Handler;

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
