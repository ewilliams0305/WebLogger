using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WebLogger.Commands
{
    /// <summary>
    /// The help command handler displays all the commands stored by an instance of the web logger
    /// </summary>
    internal sealed class HelpCommandCommand : IWebLoggerCommand
    {
        private readonly IWebLoggerCommander _logger;

        /// <summary>
        /// Console Command to send
        /// </summary>
        public string Command => "HELP";

        /// <summary>
        /// Description of command
        /// </summary>
        public string Description => "Returns all registered WebLogger console commands";

        /// <summary>
        /// Help summary explaining parameters
        /// </summary>
        public string Help => "Parameter: NA";

        /// <summary>
        /// The callback function invoked when the console command is received. 
        /// </summary>
        public Func<string, List<string>, ICommandResponse> CommandHandler => HandleCommand;

        /// <summary>
        /// Creates a help command handler.
        /// </summary>
        /// <param name="logger"></param>
        public HelpCommandCommand(IWebLoggerCommander logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// The command handle that will be executed when the command is parsed from the weblogger
        /// </summary>
        /// <param name="command">Name of the command being handled.</param>
        /// <param name="args">Collected args received from the command.</param>
        public ICommandResponse HandleCommand(string command, List<string> args)
        {
            var commands = _logger.ListCommands();

            var builder =
                new StringBuilder(
                        $@"<br><span style="" color:#FF00FF; "">{"COMMAND".PadRight(22, '.')} | {"HELP".PadRight(60, '.')}  </>")
                    .Append("<br>");
            
            foreach (var cmd in commands)
            {
                builder.Append(
                        $@"<span style="" color:#dddd11; "">>&nbsp;{cmd.Command.ToUpper().PadRight(20, '.')} | </><span style=""color:#FFF;"">{cmd.Description.ToUpper().PadRight(40, '.')} | </><span style=""color:#FFF;"">{cmd.Help} | </>")
                    .Append("<br>");
            }
            builder.Append("<br>");
            return CommandResponse.Success(this, builder.ToString());
        }
    }
}
