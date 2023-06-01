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
                            options.DestinationWebpageDirectory = Path.Combine(Directory.GetApplicationRootDirectory(), "html/logger");
                            options.WebSocketTcpPort = 54321;
                        },
                        logger =>
                        {
                            logger.ServeWebLoggerHtml(8081)
                                .DiscoverProvidedCommands()
                                .DiscoverCrestronCommands();

                        })
                    .CreateLogger();

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