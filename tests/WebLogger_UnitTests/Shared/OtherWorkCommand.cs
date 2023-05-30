namespace WebLogger_UnitTests;

internal class OtherWorkCommand : IWebLoggerCommand
{
    public string Command => "OTHER";
    public string Description => "Does work";
    public string Help => "Does lots of stuff";
    public Func<string, List<string>, string> CommandHandler => DoOtherWork;

    public OtherWorkCommand()
    {
            
    }

    public string DoOtherWork(string command, List<string> args)
    {
        return "The work was done";
    }
}