using System;
using System.Drawing;
using System.Text;
using Serilog.Events;
using WebLogger.Render;

namespace WebLogger
{
    public sealed class RenderHtml
    {
        private static HtmlElement CreatePrefix(LogEvent logEvent, Color color)
        {
            var builder = new StringBuilder("[")
                .Append(logEvent.Timestamp.ToString("HH:mm:ss"))
                .Append(" ")
                .Append(logEvent.Level.GetLevel())
                .Append("] ");

            return HtmlElement.SpanElement(builder.ToString(), color);
        }

        public static string RenderVerbose(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);

            return CreatePrefix(logEvent, color)
                .Append(logEvent.RenderMessage(formatProvider))
                .Render();
        }

        public static string RenderInformation(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Verbose);

            return CreatePrefix(logEvent, color)
                .Append(logEvent.RenderMessage(formatProvider))
                .Render();
        }
        public static string RenderWarning(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Warning);

            return CreatePrefix(logEvent, color)
                .Append(logEvent.RenderMessage(formatProvider))
                .Render();
        }

        public static string RenderError(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var color = ColorFactory.Instance.GetColor(Severity.Error);

            if (logEvent.Exception != null)
            {
                return CreatePrefix(logEvent, color)
                    .Append(logEvent.RenderMessage(formatProvider))
                    .Append(HtmlElement.SpanElement(" Exception: ", color))
                    .Append(RenderExceptions(logEvent.Exception, color))
                    .Render();
            }

            return CreatePrefix(logEvent, color)
                .Append(HtmlElement.SpanElement(logEvent.RenderMessage(formatProvider), color))
                .Render();
        }

        private static HtmlElement RenderExceptions(Exception exception, Color color)
        {
            var header = HtmlElement.SpanElement(exception.Message);

            var stacktrace = HtmlElement.ParagraphElement(exception.StackTrace
                    .Replace('<', '|')
                    .Replace('>', '|'),
                color);

            return header.Append(HtmlElement.ParagraphElement(stacktrace, color));
        }
    }
}