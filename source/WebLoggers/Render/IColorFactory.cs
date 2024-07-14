using System;
using System.Drawing;

namespace WebLogger.Render
{
    /// <summary>
    /// Stores and returns colors used to render messages
    /// </summary>
    public interface IColorFactory
    {
        /// <summary>
        /// Provides new colors to the color factory.  Default Colors are already defined and will be provided for you.
        /// </summary>
        /// <param name="verbose">Verbose message color</param>
        /// <param name="information">Information message color</param>
        /// <param name="warning">Warning message color</param>
        /// <param name="error">Error message color</param>
        void ProvideColors(Color verbose = default, Color information = default, Color warning = default, Color error = default);

        /// <summary>
        /// Returns the color matching the severity
        /// </summary>
        /// <param name="severity">Level of the message</param>
        /// <returns>A Color</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Color GetColor(Severity severity);
    }
}