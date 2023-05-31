using System;
using System.Collections.Generic;
using System.Text;
using WebLogger.Exceptions;
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
        private readonly List<string> _backlog = new List<string>();

        private Action<CloseEventArgs> _connectionClosedHandler;
        private Action<ErrorEventArgs> _connectionErrorArgs;

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
            _connectionClosedHandler = connectionClosedHandler;
            _connectionErrorArgs = connectionErrorArgs;
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            _connected = true;

            SendSerial("\rWEB LOGGER> CONNECTED TO CONSOLE");

            if (_backlog.Count > 0)
            {
                foreach (var msg in _backlog)
                    SendSerial(msg);
            }

            _backlog.Clear();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            _connected = false;
            _connectionClosedHandler?.Invoke(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            _connectionErrorArgs?.Invoke(e);
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

        private string FormatSuccessMessage(string command, string message)
        {
            var builder = new StringBuilder($@"<br><span style="" color:#FF00FF; "">")
                .Append(command.ToUpper())
                .Append(">")
                .Append(@"</><span style=""color:#FFF;"">")
                .Append(message)
                .Append("</>");

            return builder.ToString();
        }
        private string FormatFailureMessage(string command, string message)
        {
            var header = $@"<br><span style="" color:#FF00FF; "">{"COMMAND".PadRight(22, '.')} | {"HELP".PadRight(60, '.')}  </>";
            var builder = new StringBuilder($@"<br><span style="" color:rgb(242, 91, 91); "">")
                .Append(command.ToUpper())
                .Append(">")
                .Append(@"</><span style=""color:#FFF;"">")
                .Append(message)
                .Append("</>");

            return builder.ToString();
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

            else
                _backlog.Add(text);
        }

        public void WriteLine<T>(string msg, T arg)
        {
            var text = string.Format(msg, arg);

            if (_connected)
                SendSerial(text);

            else
                _backlog.Add(text);
        }
        
        public void WriteLine<T1, T2>(string msg, T1 arg1, T2 arg2)
        {
            var text = string.Format(msg, new object[]{arg1, arg2});

            if (_connected)
                SendSerial(text);

            else
                _backlog.Add(text);
        }

        public void WriteLine<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3)
        {
            var text = string.Format(msg, new object[] { arg1, arg2, arg3 });

            if (_connected)
                SendSerial(text);

            else
                _backlog.Add(text);
        }
    }
}
