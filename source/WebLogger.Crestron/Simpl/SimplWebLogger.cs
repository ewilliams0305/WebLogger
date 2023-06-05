using WebSocketSharp;

namespace WebLogger.Crestron.Simpl
{
    /// <summary>
    /// Simpl Windows Wrapper
    /// </summary>
    public class SimplWebLogger
    {
        /// <summary>
        /// Configures the singleton instance for SIMPL
        /// </summary>
        /// <param name="port"></param>
        /// <param name="secure"></param>
        public void Initialize(ushort port, ushort secure)
        {
            LazyWebLogger.Instance.Initialize(port, secure);
        }
        /// <summary>
        /// Starts the server.
        /// </summary>
        public void StartServer()
        {
            LazyWebLogger.Instance.StartServer();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void StopServer()
        {
            LazyWebLogger.Instance.StopServer();
        }
    }
}
