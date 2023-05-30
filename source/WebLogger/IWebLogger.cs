using System;
using System.Reflection;

namespace WebLogger
{
    /// <summary>
    /// Provides the public API contract of the web logger console.
    /// </summary>
    public interface IWebLogger : IWebLoggerCommander, IDisposable
    {
        /// <summary>
        /// True when the web logger has been configured and is in a valid state
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Returns the path of the HTML directory storing the HTML files.
        /// </summary>
        string HtmlDirectory { get; }
        
        /// <summary>
        /// Starts the server
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server
        /// </summary>
        void Stop();

        /// <summary>
        /// Writes a message to the output with optional parameters array
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        void WriteLine(string msg, params object[] args);

        /// <summary>
        /// Writes a message to the output with optional parameters
        /// <remarks>Prevents boxing and unboxing and is preferable to the object array</remarks>
        /// </summary>
        /// <typeparam name="T">Argument property Type</typeparam>
        /// <param name="msg"></param>
        /// <param name="property">Property</param>
        void WriteLine<T>(string msg, T property);

        /// <summary>
        /// Writes a message to the output with optional parameters
        /// <remarks>Prevents boxing and unboxing and is preferable to the object array</remarks>
        /// </summary>
        /// <typeparam name="T1">1st Argument property Type</typeparam>
        /// <typeparam name="T2">1st Argument property Type</typeparam>
        /// <param name="msg"></param>
        /// <param name="property1">Property 1</param>
        /// <param name="property2">Property 2</param>
        void WriteLine<T1, T2>(string msg, T1 property1, T2 property2 = default);

        /// <summary>
        /// Writes a message to the output with optional parameters
        /// <remarks>Prevents boxing and unboxing and is preferable to the object array</remarks>
        /// </summary>
        /// <typeparam name="T1">1st Argument property Type</typeparam>
        /// <typeparam name="T2">1st Argument property Type</typeparam>
        /// <typeparam name="T3">1st Argument property Type</typeparam>
        /// <param name="msg"></param>
        /// <param name="property1">Property 1</param>
        /// <param name="property2">Property 2</param>
        /// <param name="property3">Property 3</param>
        void WriteLine<T1, T2, T3>(string msg, T1 property1, T2 property2, T3 property3 = default);
    }
}