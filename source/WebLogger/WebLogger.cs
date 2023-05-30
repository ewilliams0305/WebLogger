using System;
using System.IO;
using System.Reflection;
using WebLogger.Commands;
using WebLogger.Utilities;
using WebSocketSharp.Server;

namespace WebLogger
{
    /// <summary>
    /// Websocket server designed to provide an accessible console application
    /// </summary>
    internal sealed class WebLogger : IWebLogger
    {
        #region Static

        private static object _lock = new object();

        #endregion

        #region Private Fields

        private readonly int _port;
        private readonly bool _secured;
        private WebSocketServer _server;
        private WebLoggerBehavior _logger;
        private readonly WebLoggerCommands _commands;

        #endregion

        /// <summary>
        /// True when the web logger has been configured and is in a valid state
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Returns the path of the HTML directory storing the HTML files.
        /// </summary>
        public string HtmlDirectory { get; }

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
            _commands = new WebLoggerCommands();

            HtmlDirectory = destinationWebpageDirectory;
        }

        private void InitializeWeblogger()
        {
            lock (_lock)
            {
                if (IsInitialized) 
                    return;

                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                ConvertEmbeddedResources(HtmlDirectory);
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
            _logger?.WriteLine(msg, args);
        }
        /// <inheritdoc />
        public void WriteLine<T>(string msg, T property)
        {
            _logger?.WriteLine(msg, property);
        }
        /// <inheritdoc />
        public void WriteLine<T1, T2>(string msg, T1 property1, T2 property2 = default)
        {
            _logger?.WriteLine(msg, property1, property2);
        }
        /// <inheritdoc />
        public void WriteLine<T1, T2, T3>(string msg, T1 property1, T2 property2, T3 property3 = default)
        {
            _logger?.WriteLine(msg, property1, property2, property3);
        }

        /// <inheritdoc />
        public bool RegisterCommand(IWebLoggerCommand command)
        {
            return _commands.RegisterCommand(command);
        }

        /// <inheritdoc />
        public bool RemoveCommand(IWebLoggerCommand command)
        {
            return _commands.RemoveCommand(command);
        }

        /// <inheritdoc />
        public bool RemoveCommand(string name)
        {
            return _commands.RemoveCommand(name);
        }

        /// <summary>
        /// Help Console command Handler
        /// Prints all Registered commands to the console
        /// </summary>
        public void ListCommands()
        {
            var header = $@"<br><span style="" color:#FF00FF; "">{"COMMAND".PadRight(22, '.')} | {"HELP".PadRight(60, '.')}  </>";
            _logger?.WriteLine(header);

            foreach (var cmd in _commands.GetAllCommands())
            {
                var result = $@"<span style="" color:#dddd11; "">>&nbsp;{ cmd.Command.ToUpper().PadRight(20, '.')} | </><span style=""color:#FFF;"">{cmd.Description.ToUpper().PadRight(40, '.')} | </><span style=""color:#FFF;"">{cmd.Help} | </>";
                
                _logger?.WriteLine(result);
            }
            _logger?.WriteLine("<br>");
        }

        /// <inheritdoc />
        public string GetHelpInfo(string command)
        {
            return _commands.GetHelpInfo(command);
        }

        /// <inheritdoc />
        public bool ExecuteCommand(string command, out string response)
        {
            return _commands.ExecuteCommand(command, out response);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the web socket server.
        /// <exception cref="ArgumentException">Throws an argument exception when the port, or path are invalid</exception>
        /// </summary>
        private void CreateServer()
        {
            try
            {
                _server = new WebSocketServer(_port, _secured)
                {
                    ReuseAddress = true
                };
                _server.AddWebSocketService<WebLoggerBehavior>("/", behavior =>
                {
                    _logger = behavior;
                    _logger.InitializeBehavior(this);
                });

                _commands.RegisterCommand(new HelpCommandHandler(this));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        private void ConvertEmbeddedResources(string destinationWebpageDirectory)
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(Path.Combine(destinationWebpageDirectory, ConstantValues.HtmlIndex)))
                        return;

                    EmbeddedResources.ExtractEmbeddedResource(
                        Assembly.GetAssembly(typeof(IAssemblyMarker)),
                        ConstantValues.HtmlRoot,
                        destinationWebpageDirectory);
                }
                catch (FileLoadException)
                {
                    throw;
                }
                catch (FileNotFoundException)
                {
                    throw;
                }
                catch (IOException)
                {
                    throw;
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

            _commands.Dispose();
        }

        /// <inheritdoc />
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
