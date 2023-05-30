namespace WebLogger_UnitTests;

internal class NotCommand
{
    public string Command => "INVALID";
    public string Description => "Does work";
    public string Help => "Does lots of stuff";
    public Func<string, List<string>, string> CommandHandler => DoTheWork;

    public NotCommand()
    {
            
    }

    public string DoTheWork(string command, List<string> args)
    {
        return "The work was done";
    }
}