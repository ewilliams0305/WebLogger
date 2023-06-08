namespace WebLogger.ConsoleApp.GeneratedCommands
{
    public partial class GeneratedCommand
    {

        [CommandHandler(
            "Generated Command",
            "A new class generating a custom command",
            "This does nothing but prove something cool could happen.")]
        public ICommandResponse ExecutedMethod(string command, List<string> args)
        {
            return CommandResponse.Success(command, "WOW, something cool just happened.");
        }
    }
}


