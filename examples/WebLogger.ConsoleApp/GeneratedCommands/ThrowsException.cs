namespace WebLogger.ConsoleApp.GeneratedCommands
{
    public partial class ThrowsException
    {

        [CommandHandler(
            "Throws Exception",
            "The generated class will catch and return your exception",
            "This one will break something!")]
        public ICommandResponse ExecutedMethod(string command, List<string> args)
        {
            throw new Exception("As expected this broke!");
        }
    }
}

