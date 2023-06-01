using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebLogger.Utilities;

namespace WebLogger.Commands
{
    /// <summary>
    /// The help command handler displays all the commands stored by an instance of the web logger
    /// </summary>
    internal sealed class UpdateWebPageCommand : IWebLoggerCommand
    {
        /// <summary>
        /// Console Command to send
        /// </summary>
        public string Command => "UPDATE";

        /// <summary>
        /// Description of command
        /// </summary>
        public string Description => "Forces the webpage update and overwrites the existing webpage";

        /// <summary>
        /// Help summary explaining parameters
        /// </summary>
        public string Help => "Parameter: Destination File Path";

        /// <summary>
        /// The callback function invoked when the console command is received. 
        /// </summary>
        public Func<string, List<string>, ICommandResponse> CommandHandler => HandleCommand;

        /// <summary>
        /// Creates a updater command handler.
        /// </summary>
        public UpdateWebPageCommand()
        {
            
        }


        /// <summary>
        /// The command handle that will be executed when the command is parsed from the weblogger
        /// </summary>
        /// <param name="command">Name of the command being handled.</param>
        /// <param name="args">Collected args received from the command.</param>
        public ICommandResponse HandleCommand(string command, List<string> args)
        {
            if (args == null || args.Count == 0)
            {
                return CommandResponse.Failure(this, "Missing File Path Parameter");
            }
            try
            {
                EmbeddedResources.ExtractEmbeddedResource(
                    Assembly.GetAssembly(typeof(IAssemblyMarker)),
                    ConstantValues.HtmlRoot,
                    args[0]);

                return CommandResponse.Success(this, $"{args[0]}index.html");
            }
            catch (FileLoadException fileLoadException)
            {
                CommandResponse.Error(this, fileLoadException);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                CommandResponse.Error(this, fileNotFoundException);
            }
            catch (IOException ioException)
            {
                CommandResponse.Error(this, ioException);
            }

            return CommandResponse.Error(this, new Exception("Code Unreachable"));
        }
    }
}