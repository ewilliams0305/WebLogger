using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace VirtualControl.Debugging.WebLogger
{
    /// <summary>
    /// 
    /// </summary>
    internal class WebLoggerBehavior : WebSocketBehavior
    {
        private List<string> _backlog;
        private bool _connected = false;

        /// <summary>
        /// 
        /// </summary>
        public WebLoggerBehavior()
        {
            _backlog = new List<string>();
            _connected = false;
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            _connected = true;

            SendSerial("\rVC4> CONNECTED TO CONSOLE");

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

            foreach (var msg in e.Reason)
            {
                ErrorLog.Error("WEBSOCKET ONCLOSE: {0}", msg);
            }
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);

            ErrorLog.Error("WEBSOCKET ONERROR: {0}", e.Message);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                if(e.Data.EndsWith("?"))
                    Send("\rVC4> " + ConsoleCommands.GetHelpInfo(e.Data));

                if (!ConsoleCommands.CallCommand(e.Data))
                    Send("\rVC4> UNKNOW COMMAND");
            }
        }

        protected void SendSerial(string text)
        {
            Send(text);
        }

        public void WriteLine(string msg, params object[] args)
        {
            var text = String.Format(msg, args);

            if (_connected)
                SendSerial(text);

            else
                _backlog.Add(text);
        }

    }
}
