using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.CrestronThread;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebLogger.Crestron;
using WebLogger.Utilities;

namespace WebLogger.CrestronApp
{
    public class ControlSystem : CrestronControlSystem
    {
        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(ControllerProgramEventHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }

        public override void InitializeSystem()
        {
            try
            {
                //// Using the weblogger factory create a new instance of the weblogger server.
                //var webLogger = WebLoggerFactory.CreateWebLogger(options =>
                //{
                //    //Override the default values in the web logger options in the Action<Options> lambda
                //    options.WebSocketTcpPort = 54321;
                //    options.Secured = false;
                //    options.DestinationWebpageDirectory = Path.Combine(Directory.GetApplicationRootDirectory(), "html/logger");
                //});
                
                //// Create an HTTP file server and discover all the commands created in the crestron library.
                //webLogger
                //    .ServeWebLoggerHtml(8081)
                //    .DiscoverCrestronCommands();

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

                // Configure the serilog Sink and pass the commands into the sink configuration method.
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
                            logger
                                .ServeWebLoggerHtml(8081)
                                .DiscoverCrestronCommands();

                        })
                    .CreateLogger();

                var x = new Thread((obj) =>
                {
                    while (true)
                    {
                        Thread.Sleep(1000);

                        Log.Logger.Verbose("This is a verbose log : {Object}", "object");
                        Log.Logger.Information("This is an information log : {Object}", "object");
                        Log.Logger.Error("This is an Error log : {Object}", "object");
                    }

                }, null);

            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        }

        /// <summary>
        /// Event Handler for Programmatic events: Stop, Pause, Resume.
        /// Use this event to clean up when a program is stopping, pausing, and resuming.
        /// This event only applies to this SIMPL#Pro program, it doesn't receive events
        /// for other programs stopping
        /// </summary>
        /// <param name="programStatusEventType"></param>
        void ControllerProgramEventHandler(eProgramStatusEventType programStatusEventType)
        {
            switch (programStatusEventType)
            {
                case (eProgramStatusEventType.Paused):
                    break;
                case (eProgramStatusEventType.Resumed):
                    break;
                case (eProgramStatusEventType.Stopping):
                    Log.CloseAndFlush();
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(programStatusEventType), programStatusEventType, null);
            }

        }
    }
}