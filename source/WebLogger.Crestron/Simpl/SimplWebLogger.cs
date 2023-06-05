using WebSocketSharp;

namespace WebLogger.Crestron.Simpl
{
    public class SimplWebLogger
    {
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
