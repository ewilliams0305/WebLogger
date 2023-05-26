using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Net.Http;
using System;
using System.Collections.Generic;

namespace WebLogger.Crestron
{
    /// <summary>
    /// Serves the files in the provided directory
    /// Server is unsecured and will provide a valid html location to run the web page hosted on a secured server.
    /// Browsers will block unsecured websocket connections from html locations severed securely
    /// </summary>
    public sealed class WebLoggerHttpServer : IDisposable
    {
        #region STATIC MEMBERS

        /// <summary>
        /// The extension content types
        /// </summary>
        public static Dictionary<string, string> ExtensionContentTypes;
        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>System.String.</returns>
        public static string GetContentType(string extension)
        {
            var type = ExtensionContentTypes.ContainsKey(extension) ? ExtensionContentTypes[extension] : "text/plain";
            return type;
        }

        #endregion

        #region PRIVATE FIELDS

        private readonly HttpServer _server;
        private readonly string _directory;
        private bool _disposedValue;

        #endregion

        /// <summary>
        /// Creates a new instance of the Logo Server and starts server
        /// </summary>
        /// <param name="port">HTTP Port</param>
        /// <param name="directory">File Directory to Serve</param>
        public WebLoggerHttpServer(int port, string directory)
        {
            ExtensionContentTypes = new Dictionary<string, string>
            {
                { ".html", "text/html" },
                { ".json", "application/json" },
                { ".js", "text/javascript" },
                { ".css", "text/css" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".pdf", "application/pdf" },
                { ".png", "image/png" },
            };

            _directory = directory;
            _server = new HttpServer() { Port = port };
            _server.OnHttpRequest += Server_OnHttpRequest;
            _server.Open();

            CrestronEnvironment.ProgramStatusEventHandler += CrestronEnvironment_ProgramStatusEventHandler;
        }

        private void Server_OnHttpRequest(object sender, OnHttpRequestArgs args)
        {
            var path = args.Request.Path;

            try
            {
                if (File.Exists(_directory + path))
                {
                    var filePath = path.Replace('/', '\\');
                    var localPath = $@"{_directory}{filePath}";

                    if (File.Exists(localPath))
                    {
                        args.Response.Header.ContentType = GetContentType(new FileInfo(localPath).Extension);
                        args.Response.ContentStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
                    }
                    else
                    {
                        args.Response.ContentString = $"Not found: '{filePath}'";
                        args.Response.Code = 404;
                    }
                }
                else
                {
                    args.Response.ContentString = $"Not found: '{_directory + path}'";
                    args.Response.Code = 404;
                }
            }
            catch (Exception e)
            {
                args.Response.Code = 400;
                args.Response.ContentString = string.Format("invalid request");
            }
        }

        private void CrestronEnvironment_ProgramStatusEventHandler(eProgramStatusEventType programEventType)
        {
            if (programEventType == eProgramStatusEventType.Stopping)
                Dispose();
        }



        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            
            if (disposing)
            {
                _server.OnHttpRequest -= Server_OnHttpRequest;
                _server.Close();
                _server.Dispose();
            }

            _disposedValue = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
