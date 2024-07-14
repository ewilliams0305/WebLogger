using System;
using System.Drawing;
using System.Text;
using Serilog.Events;
using WebLogger.Render;

namespace WebLogger
{
    public sealed class RenderSinkHtml : IRenderMessages
    {
        private const string _styleyleHeader = "padding:10px;margin:10px;font-weight:bold;background-color:";
        private const string _styleyleSuffix = ";";

        private static HtmlElement CreatePrefix(LogEvent logEvent, Color color)
        {
            var styles = new StringBuilder(_styleyleHeader)
                .RenderColor(color)
                .Append(_styleyleSuffix);

            var builder = new StringBuilder(logEvent.Timestamp.ToString("HH:mm:ss"))
                .Append(" ")
                .Append(logEvent.Level.GetLevel());

            return HtmlElement.Span(builder.ToString(), new HtmlElementOptions(additionalStyles: styles.ToString()));
        }

        private string RenderMessage(LogEvent logEvent, IFormatProvider formatProvider, Color color)
        {
            return HtmlElement.Div(CreatePrefix(logEvent, color)
                    .Append(HtmlElement.Span(logEvent.RenderMessage(formatProvider))))
                .Render();
        }

        public string RenderVerbose(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);
            return RenderMessage(logEvent, formatProvider, color);
        }

        public string RenderInformation(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);

            return RenderMessage(logEvent, formatProvider, color);
        }
        public string RenderWarning(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Warning);

            return RenderMessage(logEvent, formatProvider, color);
        }

        public string RenderError(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Error);

            if (logEvent.Exception != null)
            {
                return CreatePrefix(logEvent, color)
                    .Append(logEvent.RenderMessage(formatProvider))
                    .Append(HtmlElement.Span(", Exception: "))
                    .Append(RenderExceptions(logEvent.Exception, color))
                    .Render();
            }

            return RenderMessage(logEvent, formatProvider, color);
        }

        private static HtmlElement RenderExceptions(Exception exception, Color color)
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