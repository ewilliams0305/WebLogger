using System;
using Serilog.Events;
using WebLogger.Render;

namespace WebLogger
{
    internal static class LogEventExtensions
    {

        public static Severity GetSeverity(this LogEvent logEvent)
        {
            return logEvent.Level.GetSeverity();
        }
        public static string GetLevel(this LogEvent logEvent)
        {
            return logEvent.Level.GetLevel();
        }

        public static Severity GetSeverity(this LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug:
                    return Severity.Verbose;
                case LogEventLevel.Information:
                    return Severity.Information;
                case LogEventLevel.Warning:
                    return Severity.Warning;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    return Severity.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public static string GetLevel(this LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    return FormattingConstants.Verbose;
                case LogEventLevel.Debug:
                    return FormattingConstants.Debug;
                case LogEventLevel.Information:
                    return FormattingConstants.Information;
                case LogEventLevel.Warning:
                    return FormattingConstants.Warning;
                case LogEventLevel.Error:
                    return FormattingConstants.Error;
                case LogEventLevel.Fatal:
                    return FormattingConstants.Fatal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}