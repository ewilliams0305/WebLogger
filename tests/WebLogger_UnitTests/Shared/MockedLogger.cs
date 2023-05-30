namespace WebLogger_UnitTests

{
    internal class MockedLogger : IWebLogger
    {
        private WebLoggerCommands _commands = new WebLoggerCommands();

        public Action<string> CommandWriteLine { get; set; }

        public MockedLogger()
        {
            IsInitialized = true;
        }
        public bool RegisterCommand(IWebLoggerCommand command)
        {
            return _commands.RegisterCommand(command);
        }

        public bool RemoveCommand(IWebLoggerCommand command)
        {
            return _commands.RemoveCommand(command);
        }

        public bool RemoveCommand(string name)
        {
            return _commands.RemoveCommand(name);
        }

        public void ListCommands()
        {
            
        }

        public string GetHelpInfo(string command)
        {
            return _commands.GetHelpInfo(command);
        }

        public bool ExecuteCommand(string command, out string response)
        {
            return _commands.ExecuteCommand(command, out response);
        }

        public void Dispose()
        {
            
        }

        public bool IsInitialized { get; }

        public string HtmlDirectory { get; }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }

        public void WriteLine(string msg, params object[] args)
        {
            
        }

        public void WriteLine<T>(string msg, T property)
        {
            
        }

        public void WriteLine<T1, T2>(string msg, T1 property1, T2 property2 = default)
        {
            
        }

        public void WriteLine<T1, T2, T3>(string msg, T1 property1, T2 property2, T3 property3 = default)
        {
            
        }
    }
}
