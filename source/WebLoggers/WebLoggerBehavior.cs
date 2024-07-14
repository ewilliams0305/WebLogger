using System;
using System.Text;
using WebLogger.Exceptions;
using WebLogger.Render;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebLogger
{
    /// <summary>
    /// The web socket behavior of the weblogger client.
    /// </summary>
    internal class WebLoggerBehavior : WebSocketBehavior
    {
        private IWebLoggerCommander _logger;
        private bool _connected;

        public WebLoggerBehavior()
        {

        }

        public void InitializeBehavior(IWebLoggerCommander logger)
        {
            _logger = logger;
        }

        protected override void OnOpen()
        {
            _connected = true;
            var styles  = new StringBuilder("text-align:center;padding:10px;font-weight:bold;border: 3px solid ")
                .RenderColor(ColorFactory.Instance.WarningColor)
                .Append(";");

            var options = new HtmlElementOptions(additionalStyles: styles.ToString());
            var welcome = HtmlElement.H1Element("WEBLOGGER CONSOLE", ColorFactory.Instance.WarningColor)
                .AppendLine()
                .Append(HtmlElement.Paragraph("Welcome to the WebLogger HTML console application."))
                .Append(HtmlElement.Paragraph(
                    "Enter console commands into the input field below.  Type help at anytime to display all available commands"));

            SendSerial(HtmlElement.Div(welcome, options: options).Render());
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            _connected = false;
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (!e.IsText) 
                return;

            if (TryRenderHelpMessage(e.Data, out string render))
            {
                Send(render);
                return;
            }

            var response = _logger.ExecuteCommand(e.Data);
            ProcessResponse(response);
        }

        private bool TryRenderHelpMessage(string data, out string message)
        {
            if (!data.EndsWith("?"))
            {
                message = string.Empty;
                return false;
            }
            
            var help = _logger.GetHelpInfo(data);
            message = RenderHelpMessages(help).Render();
            return true;
        }

        private static HtmlElement RenderHelpMessages(ICommandResponse response)
        {
            var builder = new StringBuilder("padding:10px;margin:10px;font-weight:bold;background-color:")
                .RenderColor(response.Status == CommandResult.Success
                    ? ColorFactory.Instance.InformationColor
                    : ColorFactory.Instance.ErrorColor)
                .Append(";");

            var options = new HtmlElementOptions(additionalStyles: builder.ToString(), fontSize:20);

            var header = HtmlElement.Span(
                HtmlElement.Span("? ").Append(
                HtmlElement.Span(response.Command)), options);

            var help = header
                .Append(HtmlElement.Span(response.Response));

            return HtmlElement.Div(help);
        }

        private void ProcessResponse(ICommandResponse response)
        {
            switch (response.Status)
            {
                case CommandResult.Success:
                    Send(FormatSuccessMessage(response.Command, response.Response));
                    break;
                case CommandResult.Failure:
                    Send(FormatFailureMessage(response.Command, response.Response));
                    break;
                case CommandResult.InternalError:
                    Send(FormatFailureMessage(response.Command, response.Response));
                    break;
                default:
                    Send(FormatFailureMessage("WEB LOGGER", "UNKNOWN COMMAND"));
                    break;
            }
        }

        protected void SendSerial(string text)
        {
            try
            {
                Send(text);
            }
            catch (Exception e)
            {
                throw new WebLoggerCommandException(text, e);
            }
        }

        public void WriteLine(string msg, params object[] args)
        {
            var text = string.Format(msg, args);

            if (_connected)
                SendSerial(text);
        }

        public void WriteLine<T>(string msg, T arg)
        {
            var text = string.Format(msg, arg);

            if (_connected)
                SendSerial(text);
        }
        
        public void WriteLine<T1, T2>(string msg, T1 arg1, T2 arg2)
        {
            var text = string.Format(msg, new object[]{arg1, arg2});

            if (_connected)
                SendSerial(text);
        }

        public void WriteLine<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3)
        {
            var text = string.Format(msg, new object[] { arg1, arg2, arg3 });

            if (_connected)
                SendSerial(text);
        }

        private static string FormatSuccessMessage(string command, string message)
        {
            var builder = new StringBuilder("padding:10px;margin:10px;font-weight:bold;background-color:")
                .RenderColor(ColorFactory.Instance.WarningColor)
                .Append(";");

            var options = new HtmlElementOptions(additionalStyles: builder.ToString());
            var span = HtmlElement.Span(command.ToUpper(), options).Append(HtmlElement.Span(message));
            return HtmlElement.Div(span).Render();
        }
        private static string FormatFailureMessage(string command, string message)
        {
            var builder = new StringBuilder("padding:10px;margin:10px;font-weight:bold;background-color:")
                .RenderColor(ColorFactory.Instance.ErrorColor)
                .Append(";");

            var options = new HtmlElementOptions(additionalStyles: builder.ToString());
            var span = HtmlElement.Span(command.ToUpper(), options).Append(HtmlElement.Span(message));
            return HtmlElement.Div(span).Render();
        }
    }
}
