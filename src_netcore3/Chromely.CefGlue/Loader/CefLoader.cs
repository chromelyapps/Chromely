// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefLoader.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using Xilium.CefGlue;

namespace Chromely.CefGlue.Loader
{
    /// <summary>
    /// Loads the necessary CEF runtime files from opensource.spotify.com
    /// Inherits detailed version information from cefbuilds/index page.
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
        /// Gets or sets the timeout for the CEF download in minutes.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public int DownloadTimeoutMinutes { get; set; } = 10;


        /// <summary>
        /// Load CEF runtime files.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Load(ChromelyPlatform platform)
        {
            Log.Info("CefLoader: Installing CEF runtime from " + CefBuildsDownloadUrl);

            var loader = new CefLoader(platform);
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                loader.GetDownloadUrl();
                if (!loader.ParallelDownload())
                {
                    loader.Download();
                }
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


        private readonly ChromelyPlatform _platform;
        private readonly Architecture _architecture;
        private readonly int _build;

        private readonly string _tempBz2File;
        private readonly string _tempTarFile;
        private readonly string _tempDirectory;

        private string _archiveName;
        private string _folderName;
        private string _downloadUrl;
        private long _downloadLength;

        private readonly int _numberOfParallelDownloads;
        private int _lastPercent;

        private CefLoader(ChromelyPlatform platform)
        {
            _platform = platform;
            _architecture = RuntimeInformation.ProcessArchitecture;
            _build = ChromelyRuntime.GetExpectedChromiumBuildNumber();
            Log.Info($"CefLoader: Load CEF for {_platform} {_architecture}, version {_build}");

            _lastPercent = 0;
            _numberOfParallelDownloads = Environment.ProcessorCount;
            
            _tempBz2File = Path.GetTempFileName();
            _tempTarFile = Path.GetTempFileName();
            _tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="processArchitecture"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        public static string FindCefArchiveName(ChromelyPlatform platform, Architecture processArchitecture, int build)
        {
            var arch = processArchitecture.ToString()
                .Replace("X64", "64")
                .Replace("X86", "32");
            var platformIdentifier = platform.ToString().ToLower() + arch;
            var indexUrl = CefBuildsDownloadIndex(platformIdentifier);
            
            // cef_binary_3.3626.1895.g7001d56_windows64_client.tar.bz2
            var binaryNamePattern1 = $@"""(cef_binary_[0-9]+\.{build}\.[0-9]+\.(.*)_{platformIdentifier}_client.tar.bz2)""";
            
            // cef_binary_73.1.5+g4a68f1d+chromium-73.0.3683.75_windows64_client.tar.bz2
            var binaryNamePattern2 = $@"""(cef_binary_.*\+chromium\-[0-9]+\.[0-9]+\.{build}\.[0-9]_{platformIdentifier}_client.tar.bz2)""";
            
            using (var client = new WebClient())
            {
                Log.Info($"CefLoader: Load index page {indexUrl}");
                var cefIndex = client.DownloadString(indexUrl);
                // up to Chromium version 72
                var found = new Regex(binaryNamePattern1).Match(cefIndex);
                if (found.Success)
                {
                    return found.Groups[1].Value;
                }
                // from Chromium version 73 up
                found = new Regex(binaryNamePattern2).Match(cefIndex);
                if (found.Success)
                {
                    return found.Groups[1].Value;
                }
                    
                var message = $"CEF for chrome version {CefRuntime.ChromeVersion} platform {platformIdentifier} not found.";
                Log.Fatal("CefLoader: " + message);
            }
            
            return "";
        }

        private void GetDownloadUrl()
        {
            _archiveName = FindCefArchiveName(_platform, _architecture, _build);
            _folderName = _archiveName.Replace(".tar.bz2", "");
            _downloadUrl = CefDownloadUrl(_archiveName);
            Log.Info($"CefLoader: Found download URL {_downloadUrl}");
        }

        private class Range  
        {  
            public long Start { get; set; }  
            public long End { get; set; }  
        }  
        private bool ParallelDownload()
        {
            try
            {
                var webRequest = WebRequest.Create(_downloadUrl);
                webRequest.Method = "HEAD";
                using (var webResponse = webRequest.GetResponse())
                {
                    _downloadLength = long.Parse(webResponse.Headers.Get("Content-Length"));
                }

                Log.Info($"CefLoader: Parallel download {_archiveName}, {_downloadLength / (1024 * 1024)}MB");

                // Calculate ranges  
                var readRanges = new List<Range>();
                for (var chunk = 0; chunk < _numberOfParallelDownloads - 1; chunk++)
                {
                    var range = new Range()
                    {
                        Start = chunk * (_downloadLength / _numberOfParallelDownloads),
                        End = ((chunk + 1) * (_downloadLength / _numberOfParallelDownloads)) - 1
                    };
                    readRanges.Add(range);
                }
                readRanges.Add(new Range()
                {
                    Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                    End = _downloadLength - 1
                });

                // Parallel download
                var tempFilesDictionary = new ConcurrentDictionary<long, string>();

                Parallel.ForEach(readRanges, new ParallelOptions() { MaxDegreeOfParallelism = _numberOfParallelDownloads }, readRange =>
                {
                    var httpWebRequest = WebRequest.Create(_downloadUrl) as HttpWebRequest;
                    // ReSharper disable once PossibleNullReferenceException
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Timeout = (int)TimeSpan.FromMinutes(DownloadTimeoutMinutes).TotalMilliseconds;
                    httpWebRequest.AddRange(readRange.Start, readRange.End);
                    using (var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                    {
                        var tempFilePath = Path.GetTempFileName();
                        Log.Info($"CefLoader: Load {tempFilePath} ({readRange.Start}..{readRange.End})");
                        using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            httpWebResponse?.GetResponseStream()?.CopyTo(fileStream);
                            tempFilesDictionary.TryAdd(readRange.Start, tempFilePath);
                        }
                    }
                });

                // Merge to single file
                if (File.Exists(_tempBz2File))
                {
                    File.Delete(_tempBz2File);
                }
                using (var destinationStream = new FileStream(_tempBz2File, FileMode.Append))
                {
                    foreach (var tempFile in tempFilesDictionary.OrderBy(b => b.Key))
                    {
                        var tempFileBytes = File.ReadAllBytes(tempFile.Value);
                        destinationStream.Write(tempFileBytes, 0, tempFileBytes.Length);
                        File.Delete(tempFile.Value);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Fatal("CefLoader.ParallelDownload: " + ex.Message);
            }

            return false;
        }

        private void Download()
        {
            using (var client = new WebClient())
            {
                if (File.Exists(_tempBz2File))
                {
                    File.Delete(_tempBz2File);
                }

                Log.Info($"CefLoader: Loading {_tempBz2File}");
                client.DownloadProgressChanged += Client_DownloadProgressChanged;

                client.DownloadFile(_downloadUrl, _tempBz2File);
            }
        }

        private void DecompressArchive()
        {
            Log.Info("CefLoader: Decompressing BZ2 archive");
            using (var tarStream = new MemoryStream())
            {
                using (var inStream = new FileStream(_tempBz2File, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    BZip2.Decompress(inStream, tarStream, false, DecompressProgressChanged);
                }
                
                Log.Info("CefLoader: Decompressing TAR archive");
                tarStream.Seek(0, SeekOrigin.Begin);
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
            // we have to copy the 'Release' folder to the application directory
            var srcPathRelease = Path.Combine(_tempDirectory, _folderName, "Release");
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            CopyDirectory(srcPathRelease, appDirectory);
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
            var localesDir = string.Empty;
            // Create all of the sub-directories
            foreach (var dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories))
            {
                var newPath = dirPath.Replace(srcPath, dstPath);
                var dirInfo = new DirectoryInfo(newPath);
                if (!string.IsNullOrWhiteSpace(dirInfo.Name) && dirInfo.Name.Equals("locales", StringComparison.InvariantCultureIgnoreCase))
                {
                    localesDir = newPath;
                }

                Directory.CreateDirectory(newPath);
            }

             // Copy all the files & replaces any files with the same name
            foreach (var srcFile in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
            {
                var dstFile = Path.Combine(dstPath, Path.GetFileName(srcFile));
                File.Copy(srcFile, dstFile, true);

                // Copy all ***-**.pak file to locales directory
                var fileName = Path.GetFileName(dstFile);
                var ext = Path.GetExtension(dstFile);
                if (!string.IsNullOrWhiteSpace(localesDir) && !string.IsNullOrWhiteSpace(ext) && ext.ToLower().Equals(".pak") && fileName.Contains("-"))
                {
                    File.Copy(dstFile, Path.Combine(localesDir, Path.GetFileName(fileName)), true);
                }
            }
        }
    }
}
