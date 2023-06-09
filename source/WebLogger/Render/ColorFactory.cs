using System;
using System.Drawing;

namespace WebLogger.Render
{
    /// <summary>
    /// Stores and returns colors used to render messages
    /// </summary>
    public sealed class ColorFactory : IColorFactory
    {
        private static readonly Lazy<ColorFactory> Lazy = new Lazy<ColorFactory>(() => new ColorFactory());

        /// <summary>
        /// Singleton web logger instance.
        /// </summary>
        public static ColorFactory Instance => Lazy.Value;
        /// <summary>
        /// Sets the verbose color used to render messages in the console.
        /// </summary>
        public Color VerboseColor { get; private set; } = Color.Chartreuse;
        /// <summary>
        /// Sets the verbose color used to render messages in the console.
        /// </summary>
        public Color InformationColor { get; private set; } = Color.CornflowerBlue;
        /// <summary>
        /// Sets the verbose color used to render messages in the console.
        /// </summary>
        public Color WarningColor { get; private set; } = Color.DarkOrchid;
        /// <summary>
        /// Sets the verbose color used to render messages in the console.
        /// </summary>
        public Color ErrorColor { get; private set; } = Color.OrangeRed;

        /// <summary>
        /// Provides new colors to the color factory.  Default Colors are already defined and will be provided for you.
        /// </summary>
        /// <param name="verbose">Verbose message color</param>
        /// <param name="information">Information message color</param>
        /// <param name="warning">Warning message color</param>
        /// <param name="error">Error message color</param>
        public void ProvideColors(Color verbose = default, Color information = default, Color warning = default, Color error = default)
        {
            if (!verbose.IsEmpty)
            {
                VerboseColor = verbose;
            }

            if (!information.IsEmpty)
            {
                InformationColor = information;
            }

            if (!warning.IsEmpty)
            {
                WarningColor = warning;
            }

            if (!error.IsEmpty)
            {
                ErrorColor = error;
            }

        }
        /// <summary>
        /// Returns the color matching the severity
        /// </summary>
        /// <param name="severity">Level of the message</param>
        /// <returns>A Color</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Color GetColor(Severity severity)
        {
            switch (severity)
            {
                case Severity.Verbose: return VerboseColor;
                case Severity.Information:return InformationColor;
                case Severity.Warning: return WarningColor;
                case Severity.Error: return ErrorColor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }
    }
}