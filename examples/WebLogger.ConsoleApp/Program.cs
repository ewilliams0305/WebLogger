// See https://aka.ms/new-console-template for more information

using Serilog;
using System.Reflection;
using WebLogger;
using WebLogger.ConsoleApp.GeneratedCommands;
using WebLogger.Utilities;


// Optionally create a collection of commands using the provided WebLoggerCommand class
var commands = new List<IWebLoggerCommand>()
{
    new WebLoggerCommand(
        (cmd, args) => CommandResponse.Success("EXAMPLE", $"{cmd} Received"),
        "EXAMPLE",
        "Simple example of console command",
        "Parameter: NA"),

    new WebLoggerCommand(
        (cmd, args) => CommandResponse.Success("TEST", $"{cmd} Received"),
        "TEST",
        "Simple example of console command",
        "Parameter: NA")
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.WebloggerSink(
        options =>
        {
            options.Commands = commands;
            options.Secured = false;
            options.DestinationWebpageDirectory = "C:/Temp/WebLogger/Logger";
            options.WebSocketTcpPort = 54321;
        },
        logger =>
        {
            logger.DiscoverCommands(Assembly.GetAssembly(typeof(Program)))
                .DiscoverProvidedCommands();

            logger.RegisterCommand(new AnotherCommand());
        })
    .WriteTo.Console()
    .CreateLogger();

CancellationTokenSource cts = new();
CancellationToken token = cts.Token;

try
{
    Console.WriteLine($"Proceed to browser and open file: C:/Temp/WebLogger/Logger/index.html");
    await DoWork();
}
catch (Exception e)
{
    Log.Logger.Error(e, "Error in InitializeSystem: {0}", e.Message);
}
finally
{
    cts.Cancel();
    await Log.CloseAndFlushAsync();
}

async Task DoWork()
{
    while (!token.IsCancellationRequested)
    {
        await Task.Delay(1000);
    }
}

