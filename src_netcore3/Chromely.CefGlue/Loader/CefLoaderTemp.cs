using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Chromely.Core;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;

namespace Downloader
{
    /// <summary>
    /// The cef binaries downloader.
    /// </summary>
    public class CefLoaderTemp
    {
        private static string CefDownloadUrl(string name) => $"http://opensource.spotify.com/cefbuilds/{name}";

        public static void Load(ChromelyPlatform platform, string version = "77.0.3865.120")
        {
            Architecture processArchitecture = RuntimeInformation.ProcessArchitecture;

            var guid = Guid.NewGuid().ToString();
            var tempFolder = Path.Combine(Path.GetTempPath(), guid);

            try
            {
                var archiveName = KnownArchiveName(platform, processArchitecture, version);
                string downloadClientUrl = CefDownloadUrl(archiveName);
                Console.WriteLine($"From url: {downloadClientUrl}");
                string destFolder = AppDomain.CurrentDomain.BaseDirectory;

                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                var finalUnzippedName = UnzippedFolderName(downloadClientUrl);

                var bz2FileName = $"{finalUnzippedName}.tar.bz2";
                var tarFileName = $"{finalUnzippedName}.tar";

                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                var downloadedBz2File = Path.Combine(tempFolder, bz2FileName);

                var webClient = new WebClient();
                webClient.DownloadFile(downloadClientUrl, downloadedBz2File);

                var tarFile = Path.Combine(tempFolder, tarFileName);

                var bz2FileInfo = new FileInfo(downloadedBz2File);
                using (var fileToDecompressAsStream = bz2FileInfo.OpenRead())
                {
                    using (var decompressedStream = File.Create(tarFile))
                    {
                        BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
                    }
                }

                using (var inStream = File.OpenRead(tarFile))
                {
                    TarArchive tarArchive = TarArchive.CreateInputTarArchive(inStream);
                    tarArchive.ExtractContents(tempFolder);
                    tarArchive.Close();
                    inStream.Close();
                }

                DirectoryInfo tempFolderInfo = new DirectoryInfo(tempFolder);
                var finalUnzippedFolder = tempFolderInfo.GetDirectories()?.FirstOrDefault(x => x.Name.Contains(version));

                EnsureDirectoryExists(finalUnzippedFolder);
                CopyFilesFromTempFolder(platform, finalUnzippedFolder.FullName, destFolder);
            }
            catch (Exception exception)
            {
                var message = exception.GetBaseException().Message;
                Console.Error.WriteLine(message);
            }
            finally
            {
                // Delete temp folder
                var deleteTempFolderThread = new Thread(() => DeleteTempFolder(tempFolder));
                deleteTempFolderThread.Start();
            }
        }

        private static void CopyFilesFromTempFolder(ChromelyPlatform platform, string finalUnzippedFolder, string destination)
        {
            switch (platform)
            {
                case ChromelyPlatform.Windows:
                case ChromelyPlatform.Linux:
                    CopyWinLinuxFiles(finalUnzippedFolder, destination);
                    break;
                case ChromelyPlatform.MacOSX:
                    CopyMacOSFiles(finalUnzippedFolder, destination);
                    break;
            }
        }

        private static void CopyWinLinuxFiles(string finalUnzippedFolder, string destination)
        {
            EnsureDirectoryExists(finalUnzippedFolder);

            DirectoryInfo finalUnzippedFolderInfo = new DirectoryInfo(finalUnzippedFolder);
            var releaseFolderInfo = finalUnzippedFolderInfo.GetDirectories()?.FirstOrDefault(x => x.Name.Equals("Release", StringComparison.InvariantCultureIgnoreCase));
            var resourcesFolderInfo = finalUnzippedFolderInfo.GetDirectories()?.FirstOrDefault(x => x.Name.Equals("Resources", StringComparison.InvariantCultureIgnoreCase));

            EnsureDirectoryExists(releaseFolderInfo);
            EnsureDirectoryExists(resourcesFolderInfo);

            var releaseFolder = releaseFolderInfo.FullName;
            var resourcesFolder = resourcesFolderInfo.FullName;

            DirectoryCopy(releaseFolder, destination);
            DirectoryCopy(resourcesFolder, destination);
        }

        private static void CopyMacOSFiles(string finalUnzippedFolder, string destination)
        {
            EnsureDirectoryExists(finalUnzippedFolder);

            DirectoryInfo finalUnzippedFolderInfo = new DirectoryInfo(finalUnzippedFolder);
            var releaseFolderInfo = finalUnzippedFolderInfo.GetDirectories()?.FirstOrDefault(x => x.Name.Equals("Release", StringComparison.InvariantCultureIgnoreCase));

            EnsureDirectoryExists(releaseFolderInfo);

            var releaseFolder = releaseFolderInfo.FullName;

            var cefFrameworkFolder = Path.Combine(releaseFolder, "Chromium Embedded Framework.framework");

            var resourcesFolder = Path.Combine(cefFrameworkFolder, "Resources");
            var librariesFolder = Path.Combine(cefFrameworkFolder, "Libraries");

            // rename Chromium Embedded Framework to libcef.dylib and copy to destination folder
            var frameworkFile = Path.Combine(cefFrameworkFolder, "Chromium Embedded Framework");
            var libcefFile = Path.Combine(destination, "libcef.dylib");
            FileInfo libcefdylibInfo = new FileInfo(frameworkFile);
            libcefdylibInfo.CopyTo(libcefFile, true);

            // Copy Resource files
            DirectoryCopy(resourcesFolder, destination);

            // Copy Libraries files
            DirectoryCopy(librariesFolder, destination);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dirInfo = new DirectoryInfo(sourceDirName);
            EnsureDirectoryExists(dirInfo);

            DirectoryInfo[] dirs = dirInfo.GetDirectories();

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }

        private static string UnzippedFolderName(string url)
        {
            var chromiumIndex = url.IndexOf("chromium");
            return url.Substring(chromiumIndex, url.Length - (chromiumIndex + ".tar.bz2".Length));
        }

        private static void DeleteTempFolder(string folderToDelete)
        {
            try
            {
                Directory.Delete(folderToDelete, true);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static string KnownArchiveName(ChromelyPlatform platform, Architecture processArchitecture, string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return null;
            }

            var arch = processArchitecture.ToString()
                        .Replace("X64", "64")
                        .Replace("X86", "32");

            var platformIdentifier = platform.ToString().ToLower() + arch;

            switch (version)
            {
                case "77.0.3865.120":
                    return $"cef_binary_77.1.14%2Bg4fb61d2%2Bchromium-77.0.3865.120_{platformIdentifier}_minimal.tar.bz2";
            }

            return null;
        }

        private static void EnsureDirectoryExists(object folder)
        {
            if (folder == null)
            {
                throw new DirectoryNotFoundException();
            }

            var folderInfo = folder as DirectoryInfo;
            if (folderInfo != null)
            {
                folder = folderInfo.FullName;
            }

            if (!Directory.Exists(folder.ToString()))
            {
                throw new DirectoryNotFoundException($"{folder.ToString()} directory does not exist or could not be found.");
            }
        }
    }
}