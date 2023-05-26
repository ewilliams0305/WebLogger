using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebSocketSharp.Server;

namespace WebLogger
{

    /// <summary>
    /// Websocket server designed to provide an accessible console application
    /// </summary>
    public sealed class WebLogger : IDisposable
    {
        public static bool IsInitialized { get; private set; }
        private static object _lock = new object();

        private WebSocketServer _server;
        private WebLoggerBehavior _logger;

        /// <summary>
        /// Creates a new instance of the weblogger.
        /// The first instance created will convert the embedded html resources to files located in the servers HTML/Logger directory
        /// Instantiating this object is all you need to do in order to load the PWA app to the Server
        /// </summary>
        /// <param name="port">Any web socket port (default 54321)</param>
        /// <param name="secured">?</param>
        /// <param name="applicationRootDirectory">Defines where the application is running and determines where the web pages should be extracted too</param>
        public WebLogger(int port, bool secured, string applicationRootDirectory)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            if (!IsInitialized)
            {
                ConvertEmbeddedResources(applicationRootDirectory);
            }
            try
            {
                _server = new WebSocketServer(port, secured)
                {
                    ReuseAddress = true
                };
                _server.AddWebSocketService<WebLoggerBehavior>("/", behavior =>
                {
                    _logger = behavior;
                });

                ConsoleCommands.RegisterCommand(new ConsoleCommand("HELP", "Returns all registered WebLogger console commands", "Parameter: NA", GetAllCommands));
                 
            }
            catch (Exception e)

            {
                ErrorLog.Error("Exception in WebLogger constructor: {0}", e.Message);
            }
        }

        private static void ConvertEmbeddedResources(string applicationRootDirectory)
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(Path.Combine(applicationRootDirectory, "html/logger/index.html"))) 
                        return;

                    EmbeddedResources.ExtractEmbeddedResource(
                        Assembly.GetAssembly(typeof(IAssemblyMarker)),
                        ConstantValues.HtmlRoot,
                        new List<string>() { "index.html", "sw.js", "webmanifest.json" },
                        Path.Combine(applicationRootDirectory,
                            "html/logger"));

                    EmbeddedResources.ExtractEmbeddedResource(
                        Assembly.GetAssembly(typeof(IAssemblyMarker)),
                        ConstantValues.HtmlImages,
                        Path.Combine(applicationRootDirectory,
                            "html/logger/images"));

                    EmbeddedResources.ExtractEmbeddedResource(
                        Assembly.GetAssembly(typeof(IAssemblyMarker)),
                        ConstantValues.HtmlSource,
                        Path.Combine(applicationRootDirectory,
                            "html/logger/src"));
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

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Dispose();
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
        /// Prints all Registered commands to the console
        /// </summary>
        /// <param name="command">Command String</param>
        /// <param name="args">Arguments</param>
        public void GetAllCommands(string command, List<string> args)
        {
            _logger?.WriteLine("\rVC4> DISPLAYING ALL REGISTERED CONSOLE COMMANDS");

            foreach (var cmd in ConsoleCommands.GetAllCommands())
            {
                _logger?.WriteLine("\r" + cmd);
            }
        }

        private void ReleaseUnmanagedResources()
        {
            if (_server == null) 
                return;

            _server.RemoveWebSocketService("/");
            _server.Stop();
            _server = null;
            _logger = null;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;

            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~WebLogger()
        {
            ReleaseUnmanagedResources();
        }
    }
}
