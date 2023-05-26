using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WebLogger.Utilities
{
    /// <summary>
    /// Extensions to Work with Embedded Resource Files
    /// </summary>
    internal sealed class EmbeddedResources
    {

        /// <summary>
        /// Converts Embedded Resource File to Actual File On Processor
        /// </summary>
        /// <param name="assembly">Calling Assembly (Use Reflection)</param>
        /// <param name="resourceDirectory">.Syntax Project Resource Directory</param>
        /// <param name="fileName">Embedded Resource File in Project</param>
        /// <param name="outputDir">File Directory on Processor to Place Reconstructed File</param>
        /// <example>
        /// <code>
        /// EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "Cenero.Hardware.Cameras.Factory.HTML", "index.html", "HTML\\CeneroApk\\Cameras\\htmlfile");
        /// </code>
        /// </example>
        public static void ExtractEmbeddedResource(Assembly assembly, string resourceDirectory, string fileName, string outputDir)
        {
            try
            {  
                using (var stream = assembly.GetManifestResourceStream(resourceDirectory + @"." + fileName))
                {
                    using (var fileStream = new FileStream(Path.Combine(outputDir, fileName), FileMode.Create))
                    {
                        if (stream == null)
                            throw new IOException("File Not Found");

                        for (var i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e,"Failed to Convert Embedded Resource to Files: {0}", e);
            }
        }

        /// <summary>
        /// Converts Embedded Resource Files From Entire Directory to Actual Files On Processor
        /// </summary>
        /// <param name="assembly">Calling Assembly (Use Reflection)</param>
        /// <param name="resourceDirectory">.Syntax Project Resource Directory</param>
        /// <param name="outputDir">File Directory on Processor to Place Reconstructed File</param>
        /// <example>
        /// <code>
        /// EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "Cenero.Hardware.Cameras.Factory.HTML", "HTML\\CeneroApk\\Cameras\\htmlDir");
        /// </code>
        /// </example>
        public static void ExtractEmbeddedResource(Assembly assembly, string resourceDirectory, string outputDir)
        {
            
            var files = assembly.GetManifestResourceNames();

            foreach (var file in files)
            {
                try
                {
                    var fileName = file.Remove(0, resourceDirectory.Length + 1);

                    if (!file.StartsWith(resourceDirectory))
                        continue;

                    using (var stream = assembly.GetManifestResourceStream(file))
                    {
                        if (stream == null)
                            throw new IOException("File Not Found");

                        if(!Directory.Exists(outputDir))
                            Directory.CreateDirectory(outputDir);

                        var path = Path.Combine(outputDir, fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            for (int i = 0; i < stream.Length; i++)
                            {
                                fileStream.WriteByte((byte)stream.ReadByte());
                            }

                            fileStream.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Serilog.Log.Error(e, "Failed to Convert Embedded Resource {file} to Files: {message}", file, e);
                }
            }
        }

        /// <summary>
        /// Converts Embedded Resource Files From List to Actual Files On Processor
        /// </summary>
        /// <param name="assembly">Calling Assembly (Use Reflection)</param>
        /// <param name="resourceDirectory">.Syntax Project Resource Directory</param>
        /// <param name="files">List of File Names</param>
        /// <param name="outputDir">File Directory on Processor to Place Reconstructed File</param>
        /// <example>
        /// <code>
        /// EmbeddedResources.ExtractEmbeddedResource(Assembly.GetExecutingAssembly(), "Cenero.Hardware.Cameras.Factory.HTML", new List(){"index.html", "app.js", "style.css"}, "HTML\\CeneroApk\\Cameras\\htmllist");
        /// </code>
        /// </example>
        public static void ExtractEmbeddedResource(Assembly assembly, string resourceDirectory, List<string> files, string outputDir)
        {
            try
            {
                foreach (var file in files)
                {
                    using (var stream = assembly.GetManifestResourceStream(resourceDirectory + @"." + file))
                    {
                        if (stream == null)
                            throw new IOException("File Not Found");

                        using (var fileStream = new FileStream(Path.Combine(outputDir, file), FileMode.Create))
                        {
                            for (int i = 0; i < stream.Length; i++)
                            {
                                fileStream.WriteByte((byte)stream.ReadByte());
                            }
                            fileStream.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Failed to Convert Embedded Resources to Files: {0}", e);
            }
        }
    }
}