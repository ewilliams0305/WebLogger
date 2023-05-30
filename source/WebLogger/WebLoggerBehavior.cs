using System;
using System.Collections.Generic;
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

            if (_logger.ExecuteCommand(e.Data, out var response))
            {
                Send(response);
                return;
            }

            Send("\rWEB LOGGER> UNKNOWN COMMAND");
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
