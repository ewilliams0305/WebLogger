using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Reflection;
using System;
using System.Collections.Generic;
using WebSocketSharp.Server;

namespace VirtualControl.Debugging.WebLogger
{
    /// <summary>
    /// Websocket server designed to provide an accessible console application to a Crestron VC4 program instance
    /// </summary>
    public class WebLogger
    {
        public static bool IsInitialized { get; private set; }
        private static object _lock;

        private WebSocketServer _server;
        private WebLoggerBehavior _logger;

        /// <summary>
        /// Creates a new instance of the weblogger.
        /// The first instance created will convert the embedded html resources to files located in the servers HTML/Logger directory
        /// Instantiating this object is all you need to do in order to load the PWA app to the Server
        /// </summary>
        /// <param name="port">Any web socket port (default 54321)</param>
        /// <param name="secured">?</param>
        public WebLogger(int port, bool secured)
        {
            if(!IsInitialized)
            {
                _lock = new object();
                lock(_lock)
                {
                    try
                    {
                        ErrorLog.Error("EXTRACT!{0} | {1}", Directory.GetApplicationRootDirectory(), Assembly.GetExecutingAssembly());

                        if(!File.Exists(Path.Combine(Directory.GetApplicationRootDirectory(), "html/logger/index.html")))
                        {
                            EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "VirtualControl.Debugging.WebLogger.HTML", new List<string>() { "index.html", "sw.js", "webmanifest.json" }, Path.Combine(Directory.GetApplicationRootDirectory(), "Html/logger"));
                            EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "VirtualControl.Debugging.WebLogger.HTML.images", Path.Combine(Directory.GetApplicationRootDirectory(), "Html/logger/images"));
                            EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "VirtualControl.Debugging.WebLogger.HTML.src", Path.Combine(Directory.GetApplicationRootDirectory(), "Html/logger/src"));
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorLog.Exception("Failed to Convert Resource to HTML", e);
                    }
                    finally
                    {
                        _lock = null;
                        IsInitialized = true;
                    }
                }
            }
            try
            {
                _server = new WebSocketServer(port, secured);
                _server.ReuseAddress = true;

                _server.AddWebSocketService<WebLoggerBehavior>("/", () => _logger = new WebLoggerBehavior());

                ConsoleCommands.RegisterCommand(new ConsoleCommand() { Command = "HELP", Description = "Returns all registered WebLogger console commands", Help = "Parameter: NA", CommandAction = GetAllCommands });
                ConsoleCommands.RegisterCommand(new ConsoleCommand() { Command = "IPCONFIG", Description = "Returns all ethernet information", Help = "Parameter: NA", CommandAction = GetEthernetInformation });

                CrestronEnvironment.ProgramStatusEventHandler += CrestronEnvironment_ProgramStatusEventHandler;
            }
            catch (Exception e)

            {
                ErrorLog.Error("Exception in WebLogger constructor: {0}", e.Message);
            }
        }


        private void CrestronEnvironment_ProgramStatusEventHandler(eProgramStatusEventType programEventType)
        {
            if(programEventType == eProgramStatusEventType.Stopping)
            {
                _server.RemoveWebSocketService("/");
                _server.Stop();
                _server = null;
            }
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            _server.Start();
        }
        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            _server.Stop();
        }
        /// <summary>
        /// Writes a message to the client
        /// </summary>
        /// <param name="msg">String to Write</param>
        /// <param name="args">Optional Arguments</param>
        public void WriteLine(string msg, params object[] args)
        {
            if (_logger == null)
                return;

            try
            {
                _logger.WriteLine(msg, args);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in WriteLine: {0}", e.Message);
            }
        }
        /// <summary>
        /// Help Console command Handler
        /// Prints all Reginstered commands to the console
        /// </summary>
        /// <param name="command">Command String</param>
        /// <param name="args">Arguments</param>
        public void GetAllCommands(string command, List<string> args)
        {
            _logger?.WriteLine("\rVC4> DISPLAYING ALL REGISTERED CONSOLE COMMANDS");

            foreach (string cmd in ConsoleCommands.GetAllCommands())
            {
                _logger?.WriteLine("\r" + cmd);
            }
        }
        /// <summary>
        /// IpConfig command Handler
        /// Reports the servers IP/Mask/Gateway
        /// </summary>
        /// <param name="command">Command String</param>
        /// <param name="args">Arguments</param>
        public void GetEthernetInformation(string command, List<string> args)
        {
            _logger.WriteLine("DHCP: " + CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_DHCP_STATE, 0));
            _logger.WriteLine("IP ADDRESS: " + CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_ADDRESS, 0));
            _logger.WriteLine("SUBNET MASK: " + CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_MASK, 0));
            _logger.WriteLine("GATEWAY: " + CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_ROUTER, 0));
            _logger.WriteLine("MAC ADDRESS: " + CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_MAC_ADDRESS, 0));
        }
    }
}
