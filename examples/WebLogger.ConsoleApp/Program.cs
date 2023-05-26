// See https://aka.ms/new-console-template for more information

using Serilog;
using WebLogger;

// Option 1: Let the sink extension Create the instance.  When logger is closed and flushed the web logger will be disposed and stopped.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.WebloggerSink(54321, false, "C:/Temp/")
    .WriteTo.Console()
    .CreateLogger();

// Option 2: Create a logger and pass it into the Sink Extension
//var logger = new WebLogger(54321, false, "C:/Temp/");
//logger.Start();

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Verbose()
//    .WriteTo.WebloggerSink(logger)
//    .WriteTo.Console()
//    .CreateLogger();

CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

try
{

    ConsoleCommands.RegisterCommand(new ConsoleCommand(
        "EXAMPLE",
        "Simple example of console command",
        "Parameter: NA",
        (cmd, args) =>
        {
            Log.Logger.Information("{command} Received", cmd);
        }));

    ConsoleCommands.RegisterCommand(new ConsoleCommand(
        "TEST",
        "Simple example of console command",
        "Parameter: NA",
        (cmd, args) =>
        {
            Log.Logger.Information("{command} Received", cmd);
        }));

    Console.Write("Waiting!");

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

        Log.Logger.Verbose("This is a verbose log : {Object}", "object");
        Log.Logger.Information("This is an information log : {Object}", "object");
        Log.Logger.Error("This is an Error log : {Object}", "object");
    }
    
}