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
    public sealed class WebLogger : IWebLogger
    {
        #region Static
        public static bool IsInitialized { get; private set; }
        private static object _lock = new object();

        #endregion

        #region Private Fields

        private readonly int _port;
        private readonly bool _secured;
        private readonly string _destinationWebpageDirectory;
        private WebSocketServer _server;
        private WebLoggerBehavior _logger;

        #endregion


        /// <summary>
        /// Creates a new instance of the weblogger.
        /// The first instance created will convert the embedded html resources to files located in the servers HTML/Logger directory
        /// Instantiating this object is all you need to do in order to load the PWA app to the Server
        /// </summary>
        /// <param name="port">Any web socket port (default 54321)</param>
        /// <param name="secured">?</param>
        /// <param name="destinationWebpageDirectory">Defines the directory the web page will be extracted to on the running server.</param>
        public WebLogger(int port, bool secured, string destinationWebpageDirectory)
        {
            _port = port;
            _secured = secured;
            _destinationWebpageDirectory = destinationWebpageDirectory;
        }

        private void InitializeWeblogger()
        {
            lock (_lock)
            {
                if (IsInitialized) 
                    return;

                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                ConvertEmbeddedResources(_destinationWebpageDirectory);
                CreateServer();

                IsInitialized = true;
            }
        }


        #region Public Methods

        /// <summary>
        /// Starts the server
        /// </summary>
        public void Start()
        {
            InitializeWeblogger();

            if (_server == null)
                return;

            if(!_server.IsListening) 
                _server.Start();
        }
        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            if (_server == null)
                return;

            if (!_server.IsListening)
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

            _logger.WriteLine(msg, args);
        }

        public void WriteLine<T>(string msg, T property)
        {
            if (_logger == null)
                return;

            _logger.WriteLine(msg, property);
        }

        public void WriteLine<T1, T2>(string msg, T1 property1, T2 property2 = default)
        {
            if (_logger == null)
                return;

            _logger.WriteLine(msg, property1, property2);
        }

        public void WriteLine<T1, T2, T3>(string msg, T1 property1, T2 property2, T3 property3 = default)
        {
            if (_logger == null)
                return;

            _logger.WriteLine(msg, property1, property2, property3);
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

        #endregion

        #region Private Methods

        private void CreateServer()
        {
            try
            {
                _server = new WebSocketServer(_port, _secured)
                {
                    ReuseAddress = true
                };
                _server.AddWebSocketService<WebLoggerBehavior>("/", behavior => { _logger = behavior; });

                ConsoleCommands.RegisterCommand(new ConsoleCommand("HELP",
                    "Returns all registered WebLogger console commands",
                    "Parameter: NA", GetAllCommands));
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Exception in WebLogger constructor: {0}", e.Message);
            }
        }

        private static void ConvertEmbeddedResources(string destinationWebpageDirectory)
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(Path.Combine(destinationWebpageDirectory, "html/logger/index.html")))
                        return;

                    EmbeddedResources.ExtractEmbeddedResource(
                        Assembly.GetAssembly(typeof(IAssemblyMarker)),
                        ConstantValues.HtmlRoot,
                        Path.Combine(destinationWebpageDirectory, "html/logger"));
                }
                catch (Exception e)
                {
                    Serilog.Log.Error(e, "Failed to Convert Resource to HTML", e);
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


        #endregion

        #region Dispose

        private void ReleaseUnmanagedResources()
        {
            IsInitialized = false;

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

        #endregion
    }
}
