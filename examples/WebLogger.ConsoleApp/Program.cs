// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Serilog;
using Serilog.Events;
using System.Reflection;
using WebLogger;
using WebLogger.ConsoleApp.GeneratedCommands;
using WebLogger.ConsoleApp.GeneratedCommandStore;
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

//Set the default logging level and pass it into the `ControlledBy` method
var logLevelCommand = new LoggerLevelCommand(LogEventLevel.Verbose);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(logLevelCommand.LoggingLevelSwitch)
    .WriteTo.WebloggerSink(
        options =>
        {
            options.Commands = commands;
            options.Secured = false;
            options.DestinationWebpageDirectory = "C:/Temp/WebLogger/Logger";
            options.WebSocketTcpPort = 54321;

            options.Colors.ProvideColors(
                verbose: Color.Blue,
                information: Color.DarkOrange,
                error: Color.Chocolate);
        },
        logger =>
        {
            logger.DiscoverCommands(Assembly.GetAssembly(typeof(Program)))
                .DiscoverProvidedCommands();

            //Register the command and now you can change the logging level.
            logger.RegisterCommand(logLevelCommand);
            logger.RegisterCommand(new AnotherCommand());

            logger.RegisterCommandStore(new RoomControlCommandStore());

            var roomCli = new RoomControlCommandStore();
            roomCli.RegisterCommands(logger);
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
        //Log.Logger.Verbose("Verbose Log Level {name}, {something}", "Name", 2);
        //Log.Logger.Debug("Debug Log Level");
        //Log.Logger.Information("Information Log Level");
        //Log.Logger.Warning("Warning Log Level");
        //Log.Logger.Error("Error Log Level");
        //Log.Logger.Fatal(new Exception("Fake Fatal Exception", new Exception("Inner Exception", new Exception())), "Fatal Log Level");
        
    }
}

