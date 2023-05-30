namespace WebLogger_UnitTests
{
    internal class DoWorkCommand : IWebLoggerCommand
    {
        public string Command => "DO";
        public string Description => "Does work";
        public string Help => "Does lots of stuff";
        public Func<string, List<string>, string> CommandHandler => DoTheWork;

        public DoWorkCommand()
        {
            
        }

        public string DoTheWork(string command, List<string> args)
        {
            return "The work was done";
        }
    }
}
