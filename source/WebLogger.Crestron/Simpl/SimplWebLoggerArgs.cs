using System;

namespace WebLogger.Crestron.Simpl
{
    /// <summary>
    /// Event args passed to SIMPL invoked when Logger changes occur
    /// </summary>
    public sealed class SimplWebLoggerArgs : EventArgs
    {
        /// <summary>
        /// 1 = Started, 0 = Stopped
        /// </summary>
        public ushort Started { get; set; }
    }
}