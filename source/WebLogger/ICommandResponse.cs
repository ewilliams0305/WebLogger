namespace WebLogger
{
    /// <summary>
    /// Response object contract all commands need to respond with.
    /// </summary>
    public interface ICommandResponse
    {
        /// <summary>
        /// The command that is responding, provides context for the weblogger server
        /// </summary>
        string Command { get; }
        /// <summary>
        /// The response message
        /// </summary>
        string Response { get; }
        /// <summary>
        /// The status of the response
        /// </summary>
        CommandResult Status { get; }
    }
}