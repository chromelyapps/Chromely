// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core.Configuration
{
    public interface IChromelyConfiguration
    {
        string AppName { get; set; }
        string StartUrl { get; set; }
        string AppExeLocation { get; set; }
        string ChromelyVersion { get; set; }
        ChromelyPlatform Platform { get; set; }
        bool DebuggingMode { get; set; }
        string DevToolsUrl { get; set; }
        Dictionary<string, string> CommandLineArgs { get; set; }
        List<string> CommandLineOptions { get; set; }
        Dictionary<string, string> CustomSettings { get; set; }
        Dictionary<string, object> ExtensionData { get; set; }
        IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }
        List<UrlScheme> UrlSchemes { get; set; }
        CefDownloadOptions CefDownloadOptions { get; set; }
        IWindowOptions WindowOptions { get; set; }
    }
}