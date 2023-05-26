// See https://aka.ms/new-console-template for more information

using Serilog;
using WebLogger;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.WebloggerSink(54321, false, AppDomain.CurrentDomain.BaseDirectory)
    .CreateLogger();

try
{

    new ConsoleCommand()
    {
        Command = "EXAMPLE",
        Description = "Simple example of console command",
        Help = "Parameter: NA",
        CommandHandler = (cmd, args) =>
        {
            logger.WriteLine($"{cmd} Haha");
        }
    };
    ConsoleCommands.RegisterCommand(new ConsoleCommand()
    {
        Command = "EXAMPLE",
        Description = "Simple example of console command",
        Help = "Parameter: NA",
        CommandHandler = (cmd, args) =>
        {
            logger.WriteLine($"{cmd} Haha");
        }
    });
    ConsoleCommands.RegisterCommand(new ConsoleCommand()
    {
        Command = "TEST",
        Description = "this is a Test command",
        Help = "Parameter: NA",
        CommandHandler = (cmd, args) =>
        {
            logger.WriteLine($"{cmd} Haha");
        }
    });

}
catch (Exception e)
{
    ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
}