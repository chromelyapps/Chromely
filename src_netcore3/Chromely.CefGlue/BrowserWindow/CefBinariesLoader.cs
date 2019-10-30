// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CefBinariesLoader.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Chromely.CefGlue.Loader;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using Xilium.CefGlue;

namespace Chromely.CefGlue.BrowserWindow
{
    /// <summary>
    /// The cef binaries loader.
    /// </summary>
    public static class CefBinariesLoader
    {
        /// <summary>
        /// The chromely logo base6encoded.
        /// </summary>
        private const string ChromelyImgBase6Encoded = "data:image/png;base64,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";

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The list of temporary files generated
        /// </returns>
        public static List<string> Load(IChromelyConfiguration config)
        {
            try
            {
                var platform = CefRuntime.Platform;
                var version = CefRuntime.ChromeVersion;
                Logger.Instance.Log.Info($"Running {platform} chromium {version}");

                try
                {
                    CefRuntime.Load();
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log.Error(ex);
                    if (config.LoadCefBinariesIfNotFound)
                    {
                        if (config.SilentCefBinariesLoading)
                        {
                            CefLoader.Load(config.Platform);
                        }
                        else
                        {
                            return WarnUserOnLoad(config.Platform);
                        }
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error(ex);
                Environment.Exit(0);
            }

            return null;
        }

        /// <summary>
        /// The delete temp files.
        /// </summary>
        /// <param name="tempFiles">
        /// The temp files.
        /// </param>
        public static void DeleteTempFiles(List<string> tempFiles)
        {
            if (tempFiles == null || !tempFiles.Any())
            {
                return;
            }

            foreach (var file in tempFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log.Error(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// The warn user on load.
        /// </summary>
        /// <returns>
        /// The list of temporary files generated
        /// </returns>
        private static List<string> WarnUserOnLoad(ChromelyPlatform platform)
        {
            var tempFiles = new List<string>();

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var startTempFile = LaunchStartPage();
                tempFiles.Add(startTempFile);

                CefLoader.Load(platform);

                stopwatch.Stop();
                var competedTempFile = LaunchCompletedPage($"Time elapsed: {stopwatch.Elapsed}.");

                if (competedTempFile != null)
                {
                    tempFiles.Add(competedTempFile);
                }

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                Logger.Instance.Log.Error(ex);
                var onErrorTempFile = LaunchErrorPage(ex);
                tempFiles.Add(onErrorTempFile);
                Environment.Exit(0);
            }

            return tempFiles;
        }

        /// <summary>
        /// The launch start page.
        /// </summary>
        /// <returns>
        /// The temporary file created.
        /// </returns>
        private static string LaunchStartPage()
        {
            var message = new Tuple<string, string, string>(
                "CEF binaries download started.",
                "Note that depending on your network, this might take up to 4 minutes to complete.",
                "Please wait...");

            return LaunchHtmlWarningPage(message);
        }

        /// <summary>
        /// The launch completed page.
        /// </summary>
        /// <param name="durationInfo">
        /// The duration info.
        /// </param>
        /// <returns>
        /// The temporary file created.
        /// </returns>
        private static string LaunchCompletedPage(string durationInfo)
        {
            var message = new Tuple<string, string, string>(
                "CEF binaries download completed successfully.",
                durationInfo,
                "Please close.");

            return LaunchHtmlWarningPage(message);
        }

        /// <summary>
        /// The launch error page.
        /// </summary>
        /// <param name="ex">
        /// The exception.
        /// </param>
        /// <returns>
        /// The temporary file created.
        /// </returns>
        private static string LaunchErrorPage(Exception ex)
        {
            var message = new Tuple<string, string, string>(
                "CEF binaries download completed with error.",
                "Error message: " + ex.Message,
                "Please close.");

            return LaunchHtmlWarningPage(message);
        }

        /// <summary>
        /// The launch html warning page.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The temporary file created.
        /// </returns>
        private static string LaunchHtmlWarningPage(Tuple<string, string, string> message)
        {
            try
            {
                var xDocument = new XDocument(
                    new XDocumentType("html", null, null, null),
                    new XElement(
                        "html",
                                    new XAttribute("lang", "en"),
                                    new XElement(
                                        "head",
                                        new XElement(
                                            "meta",
                                            new XAttribute("http-equiv", "content-type"),
                                            new XAttribute("content", "text/html"),
                                            new XAttribute("charset", "utf-8")),
                                        new XElement("title", "Chromely CEF Binaries Download"),
                                        new XElement(
                                            "style",
                                            new XAttribute("type", "text/css"),
                                            "#box_1 { position: absolute; top: 75px; right: 75px; bottom: 75px; left: 75px; } #box_2 { position: absolute; top:150px; right: 150px; bottom: 150px; left: 150px; }")),
                                    new XElement(
                                        "body",
                                        new XElement("div", new XAttribute("id", "box_1")),
                                        new XElement(
                                            "div",
                                            new XAttribute("id", "box_2"),
                                            new XAttribute("align", "center")),
                                        new XElement(
                                            "img",
                                            new XAttribute("src", ChromelyImgBase6Encoded)),
                                        new XElement("h1", "chromely"),
                                        new XElement("div", message.Item1),
                                        new XElement("div", message.Item2),
                                        new XElement("div", message.Item3),
                                        new XElement("p", string.Empty),
                                        new XElement("div", "For more info - ", new XElement("a", "Chromely Apps", new XAttribute("href", "https://github.com/chromelyapps/Chromely"))),
                                        new XElement("p", string.Empty),
                                        new XElement(
                                            "input",
                                            new XAttribute("type", "button"),
                                            new XAttribute("value", "Close"),
                                            new XAttribute("onclick", "window.close()")))));

                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    IndentChars = "\t"
                };

                var htmlFileName = Guid.NewGuid() + ".html";

                using (var writer = XmlWriter.Create(htmlFileName, settings))
                {
                    xDocument.WriteTo(writer);
                }

                if (CefRuntime.Platform == CefRuntimePlatform.Linux)
                {
                    Process.Start("xdg-open", htmlFileName);
                }
                else if (CefRuntime.Platform == CefRuntimePlatform.MacOSX)
                {
                    Process.Start("open", htmlFileName);
                }
                else
                {
                    try
                    {
                        Process.Start(htmlFileName);
                    }
                    catch 
                    {
                        try
                        {
                            Process.Start(@"cmd.exe ", @"/c " + htmlFileName);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                return htmlFileName;
            }
            catch (Exception e)
            {
                Logger.Instance.Log.Error(e);
                Console.WriteLine(message);
            }

            return null;
        }
    }
}
