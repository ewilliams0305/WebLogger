namespace WebLogger
{
    /// <summary>
    /// Result status of the executed command.
    /// </summary>
    public enum CommandResult
    {
        /// <summary>
        /// The command executed properly and was a success
        /// </summary>
        Success,
        /// <summary>
        /// The command failed execution
        /// <remarks>This should be determined by the action of the command</remarks>
        /// </summary>
        Failure,
        /// <summary>
        /// There was a server error and the command failed to transmit.
        /// </summary>
        InternalError
    }
}