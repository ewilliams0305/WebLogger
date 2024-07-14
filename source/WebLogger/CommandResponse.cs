using System;

namespace WebLogger
{
    /// <summary>
    /// The result of all issued weblogger commands
    /// </summary>
    public readonly struct CommandResponse : ICommandResponse
    {
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Command { get; }
        /// <summary>
        /// Message to send back to the console
        /// </summary>
        public string Response { get; }
        /// <summary>
        /// Status of the command
        /// </summary>
        public CommandResult Status { get; }

        private CommandResponse(string command, string response, CommandResult status)
        {
            Command = command;
            Response = response;
            Status = status;
        }
        private CommandResponse(IWebLoggerCommand command, string response, CommandResult status)
        {
            Command = command.Command;
            Response = response;
            Status = status;
        }
        private CommandResponse(IWebLoggerCommand command, Exception exception)
        {
            Command = command.Command;
            Response = exception.Message;
            Status = CommandResult.InternalError;
        }

        /// <summary>
        /// Returns a successful response to the console.
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="response">Message to display</param>
        /// <returns>New Successful response</returns>
        public static CommandResponse Success(string command, string response)
        {
            return new CommandResponse(command, response, CommandResult.Success);
        }
        /// <summary>
        /// Returns a successful response to the console.
        /// </summary>
        /// <param name="command">The Command</param>
        /// <param name="response">Message to display</param>
        /// <returns>New Successful response</returns>
        public static CommandResponse Success(IWebLoggerCommand command, string response)
        {
            return new CommandResponse(command, response, CommandResult.Success);
        }

        /// <summary>
        /// Returns a failure response to the console.
        /// </summary>
        /// <param name="command">The Command name</param>
        /// <param name="response">Message to display</param>
        /// <returns>New failure response</returns>
        public static CommandResponse Failure(string command, string response)
        {
            return new CommandResponse(command, response, CommandResult.Failure);
        }
        /// <summary>
        /// Returns a failure response to the console.
        /// </summary>
        /// <param name="command">The Command</param>
        /// <param name="response">Message to display</param>
        /// <returns>New failure response</returns>
        public static CommandResponse Failure(IWebLoggerCommand command, string response)
        {
            return new CommandResponse(command, response, CommandResult.Failure);
        }

        /// <summary>
        /// Returns an Error response to the console.
        /// </summary>
        /// <param name="command">The Command name</param>
        /// <param name="exception">exception to display in the console</param>
        /// <returns>New Error response</returns>
        public static CommandResponse Error(IWebLoggerCommand command, Exception exception)
        {
            return new CommandResponse(command, exception);
        }
        /// <summary>
        /// Returns an Error response to the console.
        /// </summary>
        /// <param name="command">The Command name</param>
        /// <param name="message">message to display in the console</param>
        /// <returns>New Error response</returns>
        public static CommandResponse Error(IWebLoggerCommand command, string message)
        {
            return new CommandResponse(command, message, CommandResult.InternalError);
        }
        /// <summary>
        /// Returns an Error response to the console.
        /// </summary>
        /// <param name="command">The Command name</param>
        /// <param name="message">message to display in the console</param>
        /// <returns>New Error response</returns>
        public static CommandResponse Error(string command, string message)
        {
            return new CommandResponse(command, message, CommandResult.InternalError);
        }

        /// <summary>
        /// Converts a response to a result code
        /// </summary>
        /// <param name="response">The response</param>
        public static implicit operator CommandResult(CommandResponse response) => response.Status;
    }
}