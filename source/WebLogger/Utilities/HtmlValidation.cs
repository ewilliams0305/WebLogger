using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebLogger.Utilities
{
    /// <summary>
    /// Validates the HTML files loaded to the server or appliance and determines if they should be replaced.
    /// </summary>
    internal class HtmlValidation
    {

        private static bool ValidationFileExists(IWebLogger logger)
        {
            return logger != null && File.Exists(logger.HtmlDirectory);
        }


    }
}
