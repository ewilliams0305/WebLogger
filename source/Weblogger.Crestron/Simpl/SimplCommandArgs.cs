using System;

namespace WebLogger.Crestron.Simpl
{
    /// <summary>
    /// SIMPL+ console command event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class SimplCommandArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the rx data.
        /// </summary>
        /// <value>The rx data.</value>
        public string RxData { get; set; }
    }
}
