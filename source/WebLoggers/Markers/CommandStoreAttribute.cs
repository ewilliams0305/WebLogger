namespace WebLogger
{
    /// <summary>
    /// When applied to a partial class the class will implement the IStoredCommands interface.
    /// Each child method marked with the CommandStoreTarget attribute will be added to the classes command store.
    /// </summary>
    [global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CommandStoreAttribute : global::System.Attribute
    {

    }
}