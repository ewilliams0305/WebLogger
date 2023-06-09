using System;
using Serilog.Events;

namespace WebLogger
{
    public interface IRenderMessages
    {
        string RenderVerbose(LogEvent logEvent, IFormatProvider formatProvider);
        string RenderInformation(LogEvent logEvent, IFormatProvider formatProvider);
        string RenderWarning(LogEvent logEvent, IFormatProvider formatProvider);
        string RenderError(LogEvent logEvent, IFormatProvider formatProvider);
    }
}