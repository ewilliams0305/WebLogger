using Serilog.Core;
using Serilog.Events;
using System;

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
        private readonly IRenderMessages _renderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebLoggerSink"/> class.
        /// Alternatively the Serilog sink can instantiate the weblogger and manage it for the application lifetime.
        /// </summary>
        /// <param name="options">provides the weblogger factory configuration options</param>
        /// <param name="logger">the action provides access to the logger after construction and can be used to provide commands</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="renderer">an optional render you can use to override the default HTML provider</param>
        public WebLoggerSink(Action<WebLoggerOptions> options, Action<IWebLogger> logger = default, IFormatProvider formatProvider = default, IRenderMessages renderer = default)
        {
            _logger = WebLoggerFactory.CreateWebLogger(options);
            _formatProvider = formatProvider;
            _renderer = renderer ?? new RenderSinkHtml();

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
            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug:
                    _logger.WriteLine(_renderer.RenderVerbose(logEvent, _formatProvider));
                    break;
                case LogEventLevel.Information:
                    _logger.WriteLine(_renderer.RenderInformation(logEvent, _formatProvider));
                    break;
                case LogEventLevel.Warning:
                    _logger.WriteLine(_renderer.RenderWarning(logEvent, _formatProvider));
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    _logger.WriteLine(_renderer.RenderError(logEvent, _formatProvider));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(logEvent));
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
            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(this);
        }
    }
}
