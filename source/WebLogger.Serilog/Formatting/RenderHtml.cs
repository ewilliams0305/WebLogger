using System;
using System.Drawing;
using System.Text;
using Serilog.Events;
using WebLogger.Render;

namespace WebLogger
{
    /// <summary>
    /// Renders the output as an HTML element
    /// </summary>
    public sealed class RenderSinkHtml : IRenderMessages
    {

        private const string StyleHeader = "padding:10px;margin:10px;font-weight:bold;background-color:";
        private const char StyleSuffix = ';';

        private static HtmlElement CreatePrefix(LogEvent logEvent, Color color)
        {
            var styles = new StringBuilder(StyleHeader)
                .RenderColor(color)
                .Append(StyleSuffix);

            var builder = new StringBuilder(logEvent.Timestamp.ToString("HH:mm:ss"))
                .Append(' ')
                .Append(logEvent.Level.GetLevel());

            return HtmlElement.Span(builder.ToString(), new HtmlElementOptions(additionalStyles: styles.ToString()));
        }

        /// <inheritdoc />
        public string RenderVerbose(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);
            return RenderMessage(logEvent, formatProvider, color);
        }
        /// <inheritdoc />
        public string RenderInformation(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);

            return RenderMessage(logEvent, formatProvider, color);
        }
        /// <inheritdoc />
        public string RenderWarning(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Warning);

            return RenderMessage(logEvent, formatProvider, color);
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

            return RenderMessage(logEvent, formatProvider, color);
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

        private static string RenderMessage(LogEvent logEvent, IFormatProvider formatProvider, Color color)
        {
            return HtmlElement.Div(CreatePrefix(logEvent, color)
                    .Append(HtmlElement.Span(logEvent.RenderMessage(formatProvider))))
                .Render();
        }
    }
}