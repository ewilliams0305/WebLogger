using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO; 
using Crestron.SimplSharpPro;                       
using Crestron.SimplSharpPro.CrestronThread;        
using Serilog;
using System;
using System.Collections.Generic;
using WebLogger.Crestron;

//using WebLogger.Crestron;

namespace WebLogger.CrestronApp
{
    public class ControlSystem : CrestronControlSystem
    {
        /// <summary>
        /// ControlSystem Constructor. Starting point for the SIMPL#Pro program.
        /// Use the constructor to:
        /// * Initialize the maximum number of threads (max = 400)
        /// * Register devices
        /// * Register event handlers
        /// * Add Console Commands
        /// 
        /// Please be aware that the constructor needs to exit quickly; if it doesn't
        /// exit in time, the SIMPL#Pro program will exit.
        /// 
        /// You cannot send / receive data in the constructor
        /// </summary>
        public ControlSystem()
            : base()
        {
            try
            {
                Thread.MaxNumberOfUserThreads = 20;

                //Subscribe to the controller events (System, Program, and Ethernet)
                CrestronEnvironment.SystemEventHandler += new SystemEventHandler(ControllerSystemEventHandler);
                CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(ControllerProgramEventHandler);
                CrestronEnvironment.EthernetEventHandler += new EthernetEventHandler(ControllerEthernetEventHandler);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in the constructor: {0}", e.Message);
            }
        }

        /// <summary>
        /// InitializeSystem - this method gets called after the constructor 
        /// has finished. 
        /// 
        /// Use InitializeSystem to:
        /// * Start threads
        /// * Configure ports, such as serial and verisports
        /// * Start and initialize socket connections
        /// Send initial device configurations
        /// 
        /// Please be aware that InitializeSystem needs to exit quickly also; 
        /// if it doesn't exit in time, the SIMPL#Pro program will exit.
        /// </summary>
        public override void InitializeSystem()
        {
            try
            {
                var webLogger = WebLoggerFactory.CreateWebLogger(options =>
                {
                    options.WebSocketTcpPort = 54321;
                    options.Secured = false;
                    options.DestinationWebpageDirectory = Directory.GetApplicationRootDirectory();
                });
                
                webLogger.ServeWebLoggerHtml(8081);

                var commands = new List<IWebLoggerCommand>()
                {
                    new WebLoggerCommand(
                        (cmd, args) =>
                        {
                            return $"{cmd} Received";
                        },
                        "EXAMPLE",
                        "Simple example of console command",
                        "Parameter: NA"),

                    new WebLoggerCommand(
                        (cmd, args) =>
                        {
                            return $"{cmd} Received";
                        },
                        "TEST",
                        "Simple example of console command",
                        "Parameter: NA"),

                    new IpConfigCommand(webLogger)

                };

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.WebloggerSink(webLogger, commands)
                    .CreateLogger();

                Thread x = new Thread((obj) =>
                {
                    while (true)
                    {
                        Thread.Sleep(1000);

                        Log.Logger.Verbose("This is a verbose log : {Object}", "object");
                        Log.Logger.Information("This is an information log : {Object}", "object");
                        Log.Logger.Error("This is an Error log : {Object}", "object");
                    }
                    

                    return null;

                }, null);

            }
            catch (Exception e)
            {
                ErrorLog.Error("Error in InitializeSystem: {0}", e.Message);
            }
        }

        /// <summary>
        /// Event Handler for Ethernet events: Link Up and Link Down. 
        /// Use these events to close / re-open sockets, etc. 
        /// </summary>
        /// <param name="ethernetEventArgs">This parameter holds the values 
        /// such as whether it's a Link Up or Link Down event. It will also indicate 
        /// which Ethernet adapter this event belongs to.
        /// </param>
        void ControllerEthernetEventHandler(EthernetEventArgs ethernetEventArgs)
        {
            switch (ethernetEventArgs.EthernetEventType)
            {//Determine the event type Link Up or Link Down
                case (eEthernetEventType.LinkDown):
                    //Next need to determine which adapter the event is for. 
                    //LAN is the adapter is the port connected to external networks.
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {
                        //
                    }
                    break;
                case (eEthernetEventType.LinkUp):
                    if (ethernetEventArgs.EthernetAdapter == EthernetAdapterType.EthernetLANAdapter)
                    {

                    }
                    break;
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
                    //The program has been paused.  Pause all user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Resumed):
                    //The program has been resumed. Resume all the user threads/timers as needed.
                    break;
                case (eProgramStatusEventType.Stopping):
                    
                    Log.CloseAndFlush();
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(programStatusEventType), programStatusEventType, null);
            }

        }

        /// <summary>
        /// Event Handler for system events, Disk Inserted/Ejected, and Reboot
        /// Use this event to clean up when someone types in reboot, or when your SD /USB
        /// removable media is ejected / re-inserted.
        /// </summary>
        /// <param name="systemEventType"></param>
        void ControllerSystemEventHandler(eSystemEventType systemEventType)
        {
            switch (systemEventType)
            {
                case (eSystemEventType.DiskInserted):
                    //Removable media was detected on the system
                    break;
                case (eSystemEventType.DiskRemoved):
                    //Removable media was detached from the system
                    break;
                case (eSystemEventType.Rebooting):
                    //The system is rebooting. 
                    //Very limited time to preform clean up and save any settings to disk.
                    break;
            }

        }
    }
}