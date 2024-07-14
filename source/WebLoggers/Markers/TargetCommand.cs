namespace WebLogger
{
    /// <summary>
    /// When applied to a method on a partial class Targeted with WebLoggerCommandStoreAttribute the method will be added to the
    /// instance command store.
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TargetCommand : global::System.Attribute
    {
        /// <summary>
        /// Name of the command used as the command key
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Description of the command displayed in the output of the help command.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Message that describes the usage for the command.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Default constructor sets all required properties.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="description"></param>
        /// <param name="help"></param>
        public TargetCommand(
            string command,
            string description,
            string help)
        {
            Command = command;
            Description = description;
            Help = help;
        }
    }
}