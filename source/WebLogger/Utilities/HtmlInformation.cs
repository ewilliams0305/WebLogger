using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WebLogger.Utilities
{
    internal static class HtmlInformation
    {
        internal static bool VerifyRunningVersionIsSameAsLoadedVersion(string htmlDirectory)
        {
            if (!InformationFileExists(htmlDirectory))
            {
                CreateInfoFile(htmlDirectory);
            }

            var infoFile = ReadInformationFile(htmlDirectory);
            var version = ParseVersion(infoFile);

            if (!VersionsMatch(version)) 
                return true;

            ReplaceInfoFile(htmlDirectory);
            return false;

        }

        internal static bool InformationFileExists(string htmlDirectory)
        {
            var path = Path.Combine(htmlDirectory, ConstantValues.HtmlInfo);
            return File.Exists(path);
        }

        internal static string ReadInformationFile(string htmlDirectory)
        {
            var path = Path.Combine(htmlDirectory, ConstantValues.HtmlInfo);

            try
            {
                using (var reader = new StreamReader(path))
                {
                    var file = reader.ReadToEnd();

                    return file;
                }
            }
            catch (IOException e)
            {
                return string.Empty;
            }
        }

        internal static Version ParseVersion(string informationFile)
        {
            var match = Regex.Match(informationFile, @"#VERSION:(\d+\.\d+\.\d+)");

            if (!match.Success) return 
                new Version(0, 0, 0);

            var versionNumber = match.Groups[1].Value;

            return Version.TryParse(versionNumber, out var version) 
                ? version 
                : new Version(0, 0, 0);
        }

        internal static bool VersionsAreEqual(Version version1, Version version2)
        {
            if(version1 == null || version2 == null) return false;

            if(version1.Major != version2.Major) return false;

            if (version1.Minor != version2.Minor) return false;

            if (version1.Build != version2.Build) return false;

            return true;
        }

        internal static bool VersionsMatch(Version loadedVersion)
        {
            var assemblyVersion = Assembly.GetAssembly(typeof(IAssemblyMarker)).GetName().Version;

            if (assemblyVersion == null || loadedVersion == null) return false;

            return VersionsAreEqual(assemblyVersion , loadedVersion);
        }

        internal static void ReplaceInfoFile(string htmlDirectory)
        {
            var path = Path.Combine(htmlDirectory, ConstantValues.HtmlInfo);

            if (File.Exists(path))
                File.Delete(path);

            CreateInfoFile(htmlDirectory);
        }

        internal static void CreateInfoFile(string htmlDirectory)
        {
            try
            {
                var path = Path.Combine(htmlDirectory, ConstantValues.HtmlInfo);

                if(!Directory.Exists(htmlDirectory))
                    Directory.CreateDirectory(htmlDirectory);

                using (var writer = new FileStream(path, FileMode.Create))
                {
                    var builder = new StringBuilder("#VERSION:")
                        .Append(Assembly.GetAssembly(typeof(IAssemblyMarker)).GetName().Version);

                    var bytes = Encoding.UTF8.GetBytes(builder.ToString());

                    writer.Write(bytes, 0, bytes.Length);
                }
            }
            catch (IOException)
            {
                throw;
            }
            
        }
    }
}