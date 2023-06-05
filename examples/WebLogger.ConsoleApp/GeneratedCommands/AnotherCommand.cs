using WebLogger.Generators;

namespace WebLogger.ConsoleApp.GeneratedCommands
{
    public partial class AnotherCommand
    {
        public string Type { get; set; }

        public List<string> Values { get; set; } = new List<string>();

        public AnotherCommand()
        {
            Type = Command;
        }
        [CommandHandler(
            "Another",
            "Hot Damn",
            "We made a command from another something")]
        public static ICommandResponse ExecutedMethod(string command, List<string> args)
        {
            return CommandResponse.Success(command, "We build this city on source generators");
        }
    }
}

