namespace WebLogger.ConsoleApp.GeneratedCommands
{
    public partial class WayCoolCommand
    {

        [CommandHandler(
            "Way Cool",
            "Totally Tubular!",
            "Surfs up, generators making waves")]
        public ICommandResponse ExecutedMethod(string command, List<string> args)
        {
            return CommandResponse.Success(command, "Surfs up, generators making waves");
        }
    }
}