using System;
using System.Collections.Generic;
using System.Drawing;
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

        /// <summary>
        /// 
        /// </summary>
        public WebLoggerBehavior()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializeBehavior(
            IWebLoggerCommander logger, 
            Action<CloseEventArgs> connectionClosedHandler = null, 
            Action<ErrorEventArgs> connectionErrorArgs = null)
        {
            _logger = logger;
        }

        protected override void OnOpen()
        {
            _connected = true;

            SendSerial("\rWEB LOGGER> CONNECTED TO CONSOLE");
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

            if (e.Data.EndsWith("?"))
            {
                Send("\rWEB LOGGER> " + _logger.GetHelpInfo(e.Data));
                return;
            }

            var response = _logger.ExecuteCommand(e.Data);
            ProcessResponse(response);
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
