using WebLogger.Generators;

namespace WebLogger.ConsoleApp.GeneratedCommands
{
    public partial class AnotherCommand
    {
        [CommandHandler(
            "Another Command",
            "Hot Damn",
            "We made a command from nothing")]
        public ICommandResponse ExecutedMethod(string command, List<string> args)
        {
            return CommandResponse.Success(command, "We build this city on source generators");
        }
    }
}

