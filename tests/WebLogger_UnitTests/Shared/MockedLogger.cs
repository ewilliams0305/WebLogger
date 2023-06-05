namespace WebLogger_UnitTests

{
    internal class MockedLogger : IWebLogger
    {
        private readonly WebLoggerCommands _commands = new();

        public Action<string>? CommandWriteLine { get; set; }

        public MockedLogger()
        {
            IsInitialized = true;
            Port = 54321;
            IsSecured = true;
            HtmlDirectory = ConstantValues.DefaultHtmlDirectory;
        }
        public MockedLogger(int port, bool secured)
        {
            IsInitialized = true;
            Port = port;
            IsSecured = secured;
            HtmlDirectory = ConstantValues.DefaultHtmlDirectory;
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

        public IEnumerable<IWebLoggerCommand> ListCommands()
        {
            return _commands.GetAllCommands();
        }

        public string GetHelpInfo(string command)
        {
            return _commands.GetHelpInfo(command);
        }

        public ICommandResponse ExecuteCommand(string command)
        {
            return _commands.ExecuteCommand(command);
        }

        public void Dispose()
        {
            IsRunning = false;
            IsInitialized = false;
        }

        public bool IsInitialized { get; private set; }

        public string HtmlDirectory { get; }
        public bool IsSecured { get; }
        public bool IsRunning { get; private set; }
        public int Port { get; }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void WriteLine(string msg, params object[] args)
        {
            
        }

        public void WriteLine<T>(string msg, T property)
        {
            
        }

        public void WriteLine<T1, T2>(string msg, T1 property1, T2 property2)
        {
            
        }

        public void WriteLine<T1, T2, T3>(string msg, T1 property1, T2 property2, T3 property3)
        {
            
        }
    }
}
