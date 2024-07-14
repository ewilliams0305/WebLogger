using System;
using System.Drawing;
using System.Text;
using Serilog.Events;
using WebLogger.Render;

namespace WebLogger
{
    /// <summary>
    /// Generates colors used for the sink
    /// </summary>
    public sealed class RenderSinkTextColor : IRenderMessages
    {
        private static HtmlElement CreatePrefix(LogEvent logEvent, Color color)
        {
            var builder = new StringBuilder("[")
                .Append(logEvent.Timestamp.ToString("HH:mm:ss"))
                .Append(' ')
                .Append(logEvent.Level.GetLevel())
                .Append("] ");

            return HtmlElement.Span(builder.ToString(), color);
        }

        /// <inheritdoc />
        public string RenderVerbose(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);

            return CreatePrefix(logEvent, color)
                .Append(logEvent.RenderMessage(formatProvider))
                .Render();
        }
        /// <inheritdoc />
        public string RenderInformation(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);

            return CreatePrefix(logEvent, color)
                .Append(logEvent.RenderMessage(formatProvider))
                .Render();
        }
        /// <inheritdoc />
        public string RenderWarning(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Warning);

            return CreatePrefix(logEvent, color)
                .Append(logEvent.RenderMessage(formatProvider))
                .Render();
        }
        /// <inheritdoc />
        public string RenderError(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Error);

            if (logEvent.Exception != null)
            {
                return CreatePrefix(logEvent, color)
                    .Append(logEvent.RenderMessage(formatProvider))
                    .Append(HtmlElement.Span(", Exception: "))
                    .Append(RenderExceptions(logEvent.Exception))
                    .Render();
            }

            return CreatePrefix(logEvent, color)
                .Append(HtmlElement.Span(logEvent.RenderMessage(formatProvider), color))
                .Render();
        }

        private static HtmlElement RenderExceptions(Exception exception)
        {
            var builder = new StringBuilder("background-color:")
                .RenderColor(Color.DarkRed)
                .Append(";border: 3px solid rgba(255,0,0,1);");

            return HtmlElement.Table(
                HtmlElement.TableRow(
                    HtmlElement.TableData(exception.ToString(), new HtmlElementOptions(additionalStyles: builder.ToString()))
                ));
        }
    }
}