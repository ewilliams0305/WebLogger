using System;
using Serilog;
using Crestron.SimplSharp;
using Serilog.Core;
using Serilog.Events;

namespace VirtualControl.Debugging.WebLogger
{
    /// <summary>
    /// Weblogger sink that prints to the weblogger output
    /// </summary>
    /// <seealso cref="Serilog.ILogger" />
    /// <seealso cref="IFormatProvider" />
    /// <seealso cref="System.IDisposable" />
    public sealed class WebloggerSink : ILogEventSink, IDisposable
    {
        private readonly IFormatProvider _formatProvider;
        private readonly WebLogger _logger;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebloggerSink"/> class.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="logger">The logger.</param>
        public WebloggerSink(IFormatProvider formatProvider, WebLogger logger)
        {
            _formatProvider = formatProvider;
            _logger = logger;
        }
        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
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
                    ErrorLog.Notice(message);
                    break;
                case LogEventLevel.Warning:
                    _logger.WriteLine(data);
                    ErrorLog.Warn(message);
                    break;
                case LogEventLevel.Error:
                    _logger.WriteLine(data);
                    ErrorLog.Error(message);
                    break;
                case LogEventLevel.Fatal:
                    _logger.WriteLine(data);
                    ErrorLog.Exception(message, logEvent.Exception);
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
    }
}
