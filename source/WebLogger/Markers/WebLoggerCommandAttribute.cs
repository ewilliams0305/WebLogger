namespace WebLogger
{
    /// <summary>
    /// When applied to a partial class the class will be converted to an IWebLoggerCommand.
    /// The command handler will need to be marked with the CommandHandlerAttribute
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false)]
    public class WebLoggerCommandAttribute : global::System.Attribute
    {
    }
}