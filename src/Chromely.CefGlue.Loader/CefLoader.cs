using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Chromely.Core.Infrastructure;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Loader
{
    /// <summary>
    /// Loads the necessary CEF runtime files from opensource.spotify.com
    /// Inherits detailed version information from cefbuilds/index page.
    ///
    /// Note:
    /// Keep this class in a separate nuget package
    /// due to additional reference to ICSharpCode.SharpZipLib.
    /// Not everyone will be glad about this. 
    /// </summary>
    public static class CefLoader
    {
        public const string CefBuildsDownloadUrl = "http://opensource.spotify.com/cefbuilds";
        private static string CefBuildsDownloadIndex(string platform) => $"http://opensource.spotify.com/cefbuilds/index.html#{platform}_builds";
        private static string CefDownloadUrl(string name) => $"http://opensource.spotify.com/cefbuilds/{name}";

        
        // http://opensource.spotify.com/cefbuilds/cef_binary_3.3440.1805.gbe070f9_linux64_minimal.tar.bz2

        /// <summary>
        /// Load CEF runtime files.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Load()
        {
            Log.Info("CefLoader: Installing CEF runtime from " + CefBuildsDownloadUrl);

            // Do NOT use OSArchitecture but current process bitness instead
            var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture
                .ToString().ToLower().Replace("x", "");
            var platform = CefRuntime.Platform.ToString().ToLower() + arch;
            var version = string.Join(".", CefRuntime.ChromeVersion.Split('.')[2]);
            
            Log.Info($"CefLoader: Load CEF for {platform}, version {version}");

            var indexUrl = CefBuildsDownloadIndex(platform);
            var binaryNamePattern = $@"""((cef_binary_[0-9]+\.{version.Replace(".", @"\.")}\.[0-9]+\.(.*)_{platform}_minimal).tar.bz2)""";

            var tempBz2File = Path.GetTempFileName();
            var tempTarFile = Path.GetTempFileName();
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                using (var client = new WebClient())
                {
                    var index = client.DownloadString(indexUrl);
                    var found = new Regex(binaryNamePattern).Match(index);
                    if (!found.Success)
                    {
                        var message = $"CEF for chrome version {CefRuntime.ChromeVersion} platform {platform} not found.";
                        Log.Fatal("CefLoader: " + message);
                        throw new Exception(message);
                    }

                    var archiveName = found.Groups[1].Value;
                    var folderName = found.Groups[2].Value;
                    var downloadUrl = CefDownloadUrl(archiveName);

                    Log.Info($"CefLoader: Loading {archiveName}");
                    client.DownloadProgressChanged += Client_DownloadProgressChanged;
                    client.DownloadFileTaskAsync(downloadUrl, tempBz2File).Wait();

                    Log.Info("CefLoader: Decompressing BZ2 archive");
                    using (var inStream = new FileStream(tempBz2File, FileMode.Open))
                    using (var outStream = new FileStream(tempTarFile, FileMode.Create))
                    {
                        BZip2.Decompress(inStream, outStream, true, DecompressProgressChanged);
                    }
                    Log.Info("CefLoader: Decompressing TAR archive");
                    using (var tarStream = new FileStream(tempTarFile, FileMode.Open))
                    {
                        var tar = TarArchive.CreateInputTarArchive(tarStream);
                        tar.ProgressMessageEvent += (archive, entry, message) => Log.Info("CefLoader: Extracting " + entry.Name);
                        
                        Directory.CreateDirectory(tempDirectory);
                        tar.ExtractContents(tempDirectory);
                    }

                    Log.Info("CefLoader: Copy files to application directory");
                    // now we have all files in the temporary directory
                    // we have to copy the 'Release' and 'Resources' folder to the application directory
                    var srcPathRelease = Path.Combine(tempDirectory, folderName, "Release");
                    var srcPathResources = Path.Combine(tempDirectory, folderName, "Resources");
                    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    
                    CopyDirectory(srcPathRelease, appDirectory);
                    CopyDirectory(srcPathResources, appDirectory);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("CefLoader: " + ex.Message);
                throw;
            }
            finally
            {
                File.Delete(tempBz2File);
                File.Delete(tempTarFile);
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, true);
                }
            }
        }

        private static int _lastPercent;

        private static void DecompressProgressChanged(int percent)
        {
            if(percent < 10)
            {
                _lastPercent = 0;
            }
            if((percent % 10 != 0) || percent == _lastPercent)
            {
                return;   
            }
            _lastPercent = percent;
            Log.Info($"CefLoader: Decompress progress = {percent}%");
        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var percent = (int)(e.BytesReceived * 100.0 / e.TotalBytesToReceive);
            if(percent < 10)
            {
                _lastPercent = 0;
            }
            if((percent % 10 != 0) || percent == _lastPercent)
            {
                return;   
            }
            _lastPercent = percent;
            Log.Info($"CefLoader: Download progress = {percent}%");
        }

        private static void CopyDirectory(string srcPath, string dstPath)
        {
            // Create all of the sub-directories
            foreach (var dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(srcPath, dstPath));
            }

            // Copy all the files & replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(srcPath, dstPath), true);
            }
        }
        
    }
}