// Copyright � 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable SYSLIB0001
#pragma warning disable SYSLIB0014

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;

namespace Chromely.Loader;

/// <summary>
/// Loads the necessary CEF runtime files from cef-builds.spotifycdn.com
/// Inherits detailed version information from cefbuilds/index page.
/// Note:
/// Keep this class in a separate nuget package
/// due to additional reference to ICSharpCode.SharpZipLib.
/// Not everyone will be glad about this. 
/// </summary>
public class CefLoader
{
    private const string CefBuildsDownloadUrl = "https://cef-builds.spotifycdn.com";
    private static string CefBuildsDownloadIndex(string platform) => $"https://cef-builds.spotifycdn.com/index.html#{platform}_builds";
    private static string CefDownloadUrl(string name) => $"https://cef-builds.spotifycdn.com/{name}";

    private static readonly string MacOSConfigFile = "Info.plist";

    private static readonly string MacOSDefaultAppName = "Chromium Embedded Framework";

    /// <summary>
    /// Gets or sets the timeout for the CEF download in minutes.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public int DownloadTimeoutMinutes { get; set; } = 10;

    /// <summary>
    /// Download CEF runtime files.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public static void Download(ChromelyPlatform platform)
    {
        Logger.Instance.Log.LogInformation("CefLoader: Installing CEF runtime from " + CefBuildsDownloadUrl);

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
            Logger.Instance.Log.LogInformation("CefLoader: Download took {watch.ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            watch.Restart();
            loader.DecompressArchive();
            Logger.Instance.Log.LogInformation("CefLoader: Decompressing archive took {watch.ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
            watch.Restart();
            loader.CopyFilesToAppDirectory();
            Logger.Instance.Log.LogInformation("CefLoader: Copying files took {watch.ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            Logger.Instance.Log.LogError("CefLoader: {ex.Message}", ex.Message);
            throw;
        }
        finally
        {
            if (!string.IsNullOrEmpty(loader._tempBz2File))
            {
                File.Delete(loader._tempBz2File);
            }

            if (!string.IsNullOrEmpty(loader._tempTarStream))
            {
                File.Delete(loader._tempTarStream);
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
    private readonly CefBuildNumbers _build;

    private readonly string _tempTarStream;
    private readonly string _tempBz2File;
    private readonly string _tempTarFile;
    private readonly string _tempDirectory;

    private string _archiveName = string.Empty;
    private string _folderName = string.Empty;
    private string _downloadUrl = string.Empty;
    private long _downloadLength;

    private readonly int _numberOfParallelDownloads;
    private int _lastPercent;

    private CefLoader(ChromelyPlatform platform)
    {
        _platform = platform;
        _architecture = RuntimeInformation.ProcessArchitecture;
        _build = ChromelyRuntime.GetExpectedCefBuild();
        Logger.Instance.Log.LogInformation("CefLoader: Load CEF for {_platform} {_architecture}, version {_build}", _platform, _architecture, _build);

        _lastPercent = 0;
        _numberOfParallelDownloads = Environment.ProcessorCount;

        _tempTarStream = Path.GetTempFileName();
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
    public static string FindCefArchiveName(ChromelyPlatform platform, Architecture processArchitecture, CefBuildNumbers build)
    {
        var arch = processArchitecture.ToString()
            .Replace("X64", "64")
            .Replace("X86", "32");
        var platformIdentifier = (platform + arch).ToLower();
        var indexUrl = CefBuildsDownloadIndex(platformIdentifier);

        // cef_binary_3.3626.1895.g7001d56_windows64_client.tar.bz2
        var binaryNamePattern1 = $@"""(cef_binary_[0-9]+\.{build}\.[0-9]+\.(.*)_{platformIdentifier}_client.tar.bz2)""";

        // cef_binary_73.1.5+g4a68f1d+chromium-73.0.3683.75_windows64_client.tar.bz2
        // cef_binary_77.1.18+g8e8d602+chromium-77.0.3865.120_windows64_client.tar.bz2
        var versionPattern = build.CefVersion.Replace("+", "%2B");
        var binaryNamePattern2 = $@"""(cef_binary_{versionPattern}_{platformIdentifier}_minimal.tar.bz2)""";

        using (var client = new WebClient())
        {
            Logger.Instance.Log.LogInformation("CefLoader: Load index page {indexUrl}", indexUrl);
            var cefIndex = client.DownloadString(indexUrl);

            // up to Chromium version 72
            var found = new Regex(binaryNamePattern1, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Match(cefIndex);
            if (found.Success)
            {
                return found.Groups[1].Value;
            }
            // from Chromium version 73 up
            found = new Regex(binaryNamePattern2, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Match(cefIndex);
            if (found.Success)
            {
                return found.Groups[1].Value;
            }

            #region https://github.com/chromelyapps/Chromely/issues/257 

            // Hack until fixed.
            if (!string.IsNullOrEmpty(binaryNamePattern1))
            {
                if (binaryNamePattern1.Contains("96.0.4664.110"))
                {
                    return $"cef_binary_96.0.18%2Bgfe551e4%2Bchromium-96.0.4664.110_{platformIdentifier}_minimal.tar.bz2";
                }
            }

            if (!string.IsNullOrEmpty(binaryNamePattern2))
            {
                if (binaryNamePattern2.Contains("96.0.4664.110"))
                {
                    return $"cef_binary_96.0.18%2Bgfe551e4%2Bchromium-96.0.4664.110_{platformIdentifier}_minimal.tar.bz2";
                }
            }

            #endregion Hack until fixed.

            var message = $"CEF for chrome version {CefRuntime.ChromeVersion} platform {platformIdentifier} not found.";
            Logger.Instance.Log.LogError("CefLoader: {message}", message);
        }

        return "";
    }

    public static void SetMacOSAppName(IChromelyConfiguration config)
    {
        if (config.Platform == ChromelyPlatform.MacOSX)
        {
            Task.Run(() =>
            {
                try
                {
                    var appName = config.AppName;
                    if (string.IsNullOrWhiteSpace(appName))
                    {
                        appName = Assembly.GetEntryAssembly()?.GetName().Name;
                    }

                    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var pInfoFile = Path.Combine(appDirectory, MacOSConfigFile);
                    if (File.Exists(pInfoFile))
                    {
                        var pInfoFileText = File.ReadAllText(pInfoFile);
                        pInfoFileText = pInfoFileText.Replace(MacOSDefaultAppName, appName);
                        File.WriteAllText(MacOSConfigFile, pInfoFileText);
                    }
                }
                catch { }
            });
        }
    }

    private void GetDownloadUrl()
    {
        _archiveName = FindCefArchiveName(_platform, _architecture, _build);
        _folderName = _archiveName
            .Replace("%2B", "+")
            .Replace(".tar.bz2", "");
        _downloadUrl = CefDownloadUrl(_archiveName);
        Logger.Instance.Log.LogInformation("CefLoader: Found download URL {_downloadUrl}", _downloadUrl);
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
            using WebResponse webResponse = webRequest.GetResponse();
            var contentLengthVal = webResponse.Headers?.Get("Content-Length");
            if (contentLengthVal is not null)
            {
                _downloadLength = long.Parse(contentLengthVal);
            }

            Logger.Instance.Log.LogInformation("CefLoader: Parallel download {_archiveName}, {_downloadLength / (1024 * 1024)}MB", _archiveName, _downloadLength);

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
                if (WebRequest.Create(_downloadUrl) is HttpWebRequest httpWebRequest)
                {
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Timeout = (int)TimeSpan.FromMinutes(DownloadTimeoutMinutes).TotalMilliseconds;
                    httpWebRequest.AddRange(readRange.Start, readRange.End);
                    using HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                    var tempFilePath = Path.GetTempFileName();
                    Logger.Instance.Log.LogInformation("CefLoader: Load {tempFilePath} ({readRange.Start}..{readRange.End})",
                                                            tempFilePath, readRange.Start, readRange.End);
                    using FileStream fileStream = new(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Write);
                    httpWebResponse?.GetResponseStream()?.CopyTo(fileStream);
                    tempFilesDictionary.TryAdd(readRange.Start, tempFilePath);
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
            Logger.Instance.Log.LogError("CefLoader.ParallelDownload: {ex.Message}", ex.Message);
        }

        return false;
    }

    private void Download()
    {
        using WebClient client = new();
        if (File.Exists(_tempBz2File))
        {
            File.Delete(_tempBz2File);
        }

        Logger.Instance.Log.LogInformation("CefLoader: Loading {_tempBz2File}", _tempBz2File);
        client.DownloadProgressChanged += Client_DownloadProgressChanged;

        client.DownloadFile(_downloadUrl, _tempBz2File);
    }

    private void DecompressArchive()
    {
        Logger.Instance.Log.LogInformation("CefLoader: Decompressing BZ2 archive");
        using FileStream tarStream = new(_tempTarStream, FileMode.Create, FileAccess.ReadWrite);
        using (var inStream = new FileStream(_tempBz2File, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            BZip2.Decompress(inStream, tarStream, false, DecompressProgressChanged);
        }

        Logger.Instance.Log.LogInformation("CefLoader: Decompressing TAR archive");
        tarStream.Seek(0, SeekOrigin.Begin);
        var tar = TarArchive.CreateInputTarArchive(tarStream);
        tar.ProgressMessageEvent += (archive, entry, message) => Logger.Instance.Log.LogInformation("CefLoader: Extracting {entry.Name}", entry.Name);

        Directory.CreateDirectory(_tempDirectory);
        tar.ExtractContents(_tempDirectory);
    }

    private void CopyFilesToAppDirectory()
    {
        Logger.Instance.Log.LogInformation("CefLoader: Copy files to application directory");
        // now we have all files in the temporary directory
        // we have to copy the 'Release' folder to the application directory
        var srcPathRelease = Path.Combine(_tempDirectory, _folderName, "Release");
        var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        if (_platform != ChromelyPlatform.MacOSX)
        {
            CopyDirectory(srcPathRelease, appDirectory);

            var srcPathResources = Path.Combine(_tempDirectory, _folderName, "Resources");
            CopyDirectory(srcPathResources, appDirectory);
        }

        if (_platform != ChromelyPlatform.MacOSX) return;

        var cefFrameworkFolder = Path.Combine(srcPathRelease, "Chromium Embedded Framework.framework");

        // rename Chromium Embedded Framework to libcef.dylib and copy to destination folder
        var frameworkFile = Path.Combine(cefFrameworkFolder, "Chromium Embedded Framework");
        var libcefFile = Path.Combine(appDirectory, "libcef.dylib");
        var libcefdylibInfo = new FileInfo(frameworkFile);
        libcefdylibInfo.CopyTo(libcefFile, true);

        // Copy Libraries files
        var librariesFolder = Path.Combine(cefFrameworkFolder, "Libraries");
        CopyDirectory(librariesFolder, appDirectory);

        // Copy Resource files
        var resourcesFolder = Path.Combine(cefFrameworkFolder, "Resources");
        CopyDirectory(resourcesFolder, appDirectory);
    }

    private void DecompressProgressChanged(int percent)
    {
        if (percent < 10)
        {
            _lastPercent = 0;
        }
        if ((percent % 10 != 0) || percent == _lastPercent)
        {
            return;
        }
        _lastPercent = percent;
        Logger.Instance.Log.LogInformation("CefLoader: Decompress progress = {percent}%", percent);
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
        Logger.Instance.Log.LogInformation("CefLoader: Download progress = {percent}%", percent);
    }

    private static void CopyDirectory(string sourceDirName, string destDirName)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dirInfo = new(sourceDirName);
        DirectoryInfo[] dirs = dirInfo.GetDirectories();

        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        var files = dirInfo.GetFiles();
        foreach (var file in files)
        {
            var tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, true);
        }

        foreach (var subDir in dirs)
        {
            var tempPath = Path.Combine(destDirName, subDir.Name);
            CopyDirectory(subDir.FullName, tempPath);
        }
    }
}