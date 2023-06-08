using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WebLogger.Render;

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
            var commands = _logger
                .ListCommands()
                .ToList();

            commands.Sort(Comparison);

            var header = HtmlElement.SpanElement("COMMAND".PadRight(22, '.'), new HtmlElementOptions(Color.BlueViolet))
                .Append(" | ")
                .Append("HELP".PadRight(60, '.'));

            var paragraph = HtmlElement.ParagraphElement(header);

            var content = HtmlElement.DivElement(HtmlConstants.NewLine);
            
            foreach (var cmd in commands)
            {
                content.Append(
                    HtmlElement.SpanElement(cmd.Command.ToUpper().PadRight(20, '.').PadLeft(5, ' '), Color.Chartreuse)).Append(
                    HtmlElement.SpanElement(cmd.Description.ToUpper().PadRight(40, '.'), Color.Chartreuse)).Append(
                    HtmlElement.SpanElement(cmd.Help))
                    .AppendLine();
            }

            var result = paragraph.Insert(content).Render();
            return CommandResponse.Success(this, result);
        }

        private int Comparison(IWebLoggerCommand x, IWebLoggerCommand y)
        {
            return string.Compare(x.Command, y.Command, StringComparison.Ordinal);
        }
    }
}
