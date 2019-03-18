using System;
using System.Diagnostics;
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
    public class CefLoader
    {
        private const string CefBuildsDownloadUrl = "http://opensource.spotify.com/cefbuilds";
        private static string CefBuildsDownloadIndex(string platform) => $"http://opensource.spotify.com/cefbuilds/index.html#{platform}_builds";
        private static string CefDownloadUrl(string name) => $"http://opensource.spotify.com/cefbuilds/{name}";

        /// <summary>
        /// Load CEF runtime files.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Load()
        {
            Log.Info("CefLoader: Installing CEF runtime from " + CefBuildsDownloadUrl);

            var loader = new CefLoader();
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                loader.Download();
                Log.Info($"CefLoader: Download took {watch.ElapsedMilliseconds}ms");
                watch.Restart();
                loader.DecompressArchive();
                Log.Info($"CefLoader: Decompressing archive took {watch.ElapsedMilliseconds}ms");
                watch.Restart();
                loader.CopyFilesToAppDirectory();
                Log.Info($"CefLoader: Copying files took {watch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                Log.Fatal("CefLoader: " + ex.Message);
                throw;
            }
            finally
            {
                if (!string.IsNullOrEmpty(loader._tempBz2File))
                {
                    File.Delete(loader._tempBz2File);
                }

                if (!string.IsNullOrEmpty(loader._tempTarFile))
                {
                    File.Delete(loader._tempTarFile);
                }
                if (!string.IsNullOrEmpty(loader._tempDirectory) && Directory.Exists(loader._tempDirectory))
                {
                    Directory.Delete(loader._tempDirectory, true);
                }
            }
        }

        
        private readonly string _platform;
        private readonly string _indexUrl;
        private readonly string _binaryNamePattern;

        private readonly string _tempBz2File;
        private readonly string _tempTarFile;
        private readonly string _tempDirectory;

        private string _archiveName;
        private string _folderName;
        private string _downloadUrl;
        
        private int _lastPercent;

        private CefLoader()
        {
            // Do NOT use OSArchitecture but current process bitness instead
            var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture
                .ToString().ToLower().Replace("x", "");
            _platform = CefRuntime.Platform.ToString().ToLower() + arch;
            var version = string.Join(".", CefRuntime.ChromeVersion.Split('.')[2]);
            
            Log.Info($"CefLoader: Load CEF for {_platform}, version {version}");

            _lastPercent = 0;
            _indexUrl = CefBuildsDownloadIndex(_platform);
            version = version.Replace(".", @"\.");
            _binaryNamePattern = $@"""((cef_binary_[0-9]+\.{version}\.[0-9]+\.(.*)_{_platform}_minimal).tar.bz2)""";
    
            _tempBz2File = Path.GetTempFileName();
            _tempTarFile = Path.GetTempFileName();
            _tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }
        
        

        private void Download()
        {
            using (var client = new WebClient())
            {
                var index = client.DownloadString(_indexUrl);
                var found = new Regex(_binaryNamePattern).Match(index);
                if (!found.Success)
                {
                    var message = $"CEF for chrome version {CefRuntime.ChromeVersion} platform {_platform} not found.";
                    Log.Fatal("CefLoader: " + message);
                    throw new Exception(message);
                }

                _archiveName = found.Groups[1].Value;
                _folderName = found.Groups[2].Value;
                _downloadUrl = CefDownloadUrl(_archiveName);

                Log.Info($"CefLoader: Loading {_archiveName}");
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadFileTaskAsync(_downloadUrl, _tempBz2File).Wait();
            }
        }

        private void DecompressArchive()
        {
            Log.Info("CefLoader: Decompressing BZ2 archive");
            using (var inStream = new FileStream(_tempBz2File, FileMode.Open))
            using (var outStream = new FileStream(_tempTarFile, FileMode.Create))
            {
                BZip2.Decompress(inStream, outStream, true, DecompressProgressChanged);
            }
            Log.Info("CefLoader: Decompressing TAR archive");
            using (var tarStream = new FileStream(_tempTarFile, FileMode.Open))
            {
                var tar = TarArchive.CreateInputTarArchive(tarStream);
                tar.ProgressMessageEvent += (archive, entry, message) => Log.Info("CefLoader: Extracting " + entry.Name);
                        
                Directory.CreateDirectory(_tempDirectory);
                tar.ExtractContents(_tempDirectory);
            }
        }

        private void CopyFilesToAppDirectory()
        {
            Log.Info("CefLoader: Copy files to application directory");
            // now we have all files in the temporary directory
            // we have to copy the 'Release' and 'Resources' folder to the application directory
            var srcPathRelease = Path.Combine(_tempDirectory, _folderName, "Release");
            var srcPathResources = Path.Combine(_tempDirectory, _folderName, "Resources");
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    
            CopyDirectory(srcPathRelease, appDirectory);
            CopyDirectory(srcPathResources, appDirectory);
        }
        
        
        private void DecompressProgressChanged(int percent)
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

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var percent = (int)(e.BytesReceived * 100.0 / e.TotalBytesToReceive);
            if (percent < 10)
            {
                _lastPercent = 0;
            }
            if ((percent % 10 != 0) || percent == _lastPercent)
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