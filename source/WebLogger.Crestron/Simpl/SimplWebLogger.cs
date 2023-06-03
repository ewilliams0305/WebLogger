using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using System;
using WebLogger.Utilities;
using WebSocketSharp;

namespace WebLogger.Crestron.Simpl
{
    public class SimplWebLogger
    {
        public void Initialize(ushort port, ushort secure)
        {
            LazyWebLogger.Instance.Initialize(port, secure);
        }
        /// <summary>
        /// Starts the server.
        /// </summary>
        public void StartServer()
        {
            LazyWebLogger.Instance.StartServer();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void StopServer()
        {
            LazyWebLogger.Instance.StopServer();
        }
    }
    /// <summary>
    /// Creates a weblogger server to use with a SIMPL windows program
    /// </summary>
    public sealed class LazyWebLogger : IDisposable
    {
        private static readonly Lazy<LazyWebLogger> Lazy = 
            new Lazy<LazyWebLogger>(() => new LazyWebLogger());

        /// <summary>
        /// Singleton web logger instance.
        /// </summary>
        public static LazyWebLogger Instance => Lazy.Value;

        private IWebLogger _logger;

        private LazyWebLogger()
        {
            CrestronEnvironment.ProgramStatusEventHandler += CrestronEnvironment_ProgramStatusEventHandler;
        }

        private void CrestronEnvironment_ProgramStatusEventHandler(eProgramStatusEventType programEventType)
        {
            this.Dispose();
        }

        /// <summary>
        /// Initializes the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="secure">The secure.</param>
        public void Initialize(ushort port, ushort secure)
        {
            _logger = WebLoggerFactory.CreateWebLogger(options =>
            {
                options.WebSocketTcpPort = port;
                options.DestinationWebpageDirectory =
                    Path.Combine(Directory.GetApplicationRootDirectory(), "html/logger");

                options.Secured = secure > 0;
                options.Commands = SimplCommand.SimplCommands.Values;

                SimplCommand.SimplCommands.Clear();
            });
        }
        /// <summary>
        /// Starts the server.
        /// </summary>
        public void StartServer()
        {
            _logger?.Start();

            if (SimplCommand.SimplCommands.Count == 0)
                return;

            _logger.RegisterCommands(SimplCommand.SimplCommands.Values);
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void StopServer()
        {
            _logger?.Stop();
        }

        /// <summary>
        /// Writes a message to the weblogger
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            _logger?.WriteLine(message);
        }

        /// <summary>
        /// Stops and frees up the managed memory used by the weblogger
        /// </summary>
        public void Dispose()
        {
            _logger?.Dispose();

            CrestronEnvironment.ProgramStatusEventHandler -= CrestronEnvironment_ProgramStatusEventHandler;
        }
    }
}
