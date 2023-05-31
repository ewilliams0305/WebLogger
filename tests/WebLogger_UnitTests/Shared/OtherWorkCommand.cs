namespace WebLogger_UnitTests;

internal class OtherWorkCommand : IWebLoggerCommand
{
    public string Command => "OTHER";
    public string Description => "Does work";
    public string Help => "Does lots of stuff";
    public Func<string, List<string>, ICommandResponse> CommandHandler => DoOtherWork;

    public OtherWorkCommand()
    {
            
    }

    public ICommandResponse DoOtherWork(string command, List<string> args)
    {
        return CommandResponse.Success(this, "Done the Work");
    }
}