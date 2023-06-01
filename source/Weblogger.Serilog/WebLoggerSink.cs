using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using WebLogger.Utilities;

namespace WebLogger
{
    /// <summary>
    /// Weblogger sink that prints to the weblogger output
    /// </summary>
    /// <seealso cref="Serilog.ILogger" />
    /// <seealso cref="IFormatProvider" />
    /// <seealso cref="System.IDisposable" />
    public sealed class WebLoggerSink : ILogEventSink, IDisposable
    {

        private bool _disposed;
        private readonly IWebLogger _logger;
        private readonly IFormatProvider _formatProvider;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="WebLoggerSink"/> class.
        ///// Provide an instance of the weblogger to be utilized by the sink.
        ///// </summary>
        ///// <param name="formatProvider">The format provider.</param>
        ///// <param name="logger">The logger.</param>
        ///// <param name="commands"></param>
        //public WebLoggerSink(IFormatProvider formatProvider, IWebLogger logger, IEnumerable<IWebLoggerCommand> commands = null)
        //{
        //    _formatProvider = formatProvider;
        //    _logger = logger;

        //    RegisterCommands(commands);
        //    _logger.Start();
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="WebLoggerSink"/> class.
        ///// Alternatively the Serilog sink can instantiate the weblogger and manage it for the application lifetime.
        ///// </summary>
        ///// <param name="formatProvider">The format provider.</param>
        ///// <param name="port"></param>
        ///// <param name="secured"></param>
        ///// <param name="applicationDirectory"></param>
        ///// <param name="commands"></param>
        //public WebLoggerSink(IFormatProvider formatProvider, int port, bool secured, string applicationDirectory, IEnumerable<IWebLoggerCommand> commands = null)
        //{
        //    _logger = WebLoggerFactory.CreateWebLogger(options =>
        //    {
        //        options.WebSocketTcpPort = port;
        //        options.Secured = secured;
        //        options.DestinationWebpageDirectory = applicationDirectory;
        //    });
        //    _formatProvider = formatProvider;

        //    RegisterCommands(commands);
        //    _logger.Start();
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="WebLoggerSink"/> class.
        /// Alternatively the Serilog sink can instantiate the weblogger and manage it for the application lifetime.
        /// </summary>
        /// <param name="options">provides the weblogger factory configuration options</param>
        /// <param name="logger">the action provides access to the logger after construction and can be used to provide commands</param>
        /// <param name="formatProvider">The format provider.</param>
        public WebLoggerSink(Action<WebLoggerOptions> options, Action<IWebLogger> logger = default, IFormatProvider formatProvider = default)
        {
            _logger = WebLoggerFactory.CreateWebLogger(options);
            _formatProvider = formatProvider;

            logger?.Invoke(_logger);

            _logger.Start();
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// The stupid thing here that will change in the future is the color rendering is happening on the webpage script and should be moved to the format providers.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {

            var data = $"{logEvent.Timestamp} [{logEvent.Level.ToString().ToUpper()}] {logEvent.RenderMessage(_formatProvider)}";

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    _logger.WriteLine(data);
                    break;
                case LogEventLevel.Debug:
                    _logger.WriteLine(data);
                    break;
                case LogEventLevel.Information:
                    _logger.WriteLine(data);
                    break;
                case LogEventLevel.Warning:
                    _logger.WriteLine(data);
                    break;
                case LogEventLevel.Error:
                    _logger.WriteLine(data);
                    break;
                case LogEventLevel.Fatal:
                    _logger.WriteLine(data);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _logger?.Dispose();
            }

            _disposed = true;
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~WebLoggerSink()
        {

        }
    }
}
