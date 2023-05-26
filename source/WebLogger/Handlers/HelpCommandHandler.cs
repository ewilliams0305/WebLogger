﻿using System;
using System.Collections.Generic;

namespace WebLogger.Handlers
{
    /// <summary>
    /// The help command handler displays all the commands stored by an instance of the web logger
    /// </summary>
    internal sealed class HelpCommandHandler : IWebLoggerCommand, IWebLoggerHandler
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
        public Action<string, List<string>> CommandHandler => HandleCommand;

        /// <summary>
        /// Creates a help command handler.
        /// </summary>
        /// <param name="logger"></param>
        public HelpCommandHandler(IWebLoggerCommander logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// The command handle that will be executed when the command is parsed from the weblogger
        /// </summary>
        /// <param name="command">Name of the command being handled.</param>
        /// <param name="args">Collected args received from the command.</param>
        public void HandleCommand(string command, List<string> args)
        {
            _logger.ListCommands();
        }
    }
}