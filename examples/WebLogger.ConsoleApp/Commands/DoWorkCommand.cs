namespace WebLogger.ConsoleApp.Commands
{
    internal class DoWorkCommand : IWebLoggerCommand
    {
        public string Command => "DO";
        public string Description => "Does work";
        public string Help => "Does lots of stuff";
        public Func<string, List<string>, ICommandResponse> CommandHandler => DoTheWork;

        public DoWorkCommand()
        {
            
        }

        public ICommandResponse DoTheWork(string command, List<string> args)
        {
            

            return CommandResponse.Success(this, "Done the Work");
        }
    }

}
