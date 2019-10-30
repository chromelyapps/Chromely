// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyConfiguration.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.Core
{
    public interface IChromelyConfiguration
    {
        ChromelyPlatform Platform { get; set; }
        string ChromelyVersion { get; set; }
        bool LoadCefBinariesIfNotFound { get; set; }
        bool SilentCefBinariesLoading { get; set; }
        int WindowLeft { get; set; }
        int WindowTop { get; set; }
        int WindowWidth { get; set; }
        int WindowHeight { get; set; }
        bool WindowNoResize { get; set; }
        bool WindowNoMinMaxBoxes { get; set; }
        bool WindowFrameless { get; set; }
        bool WindowCenterScreen { get; set; }
        bool WindowKioskMode { get; set; }
        WindowState WindowState { get; set; }
        string WindowTitle { get; set; }
        string WindowIconFile { get; set; }
        int WindowCustomStyle { get; set; }
        bool UseWindowCustomStyle { get; set; }
        string AppExeLocation { get; set; }
        string StartUrl { get; set; }
        bool DebuggingMode { get; set; }
        List<UrlScheme> UrlSchemes { get; set; }
        List<ControllerAssemblyInfo> ControllerAssemblies { get; set; }
        List<ChromelyEventHandler<object>> EventHandlers { get; set; }
        List<Tuple<string, string>> CommandLineArgs { get; set; }
        List<string> CommandLineOptions { get; set; }
        Dictionary<string, string> CustomSettings { get; set; }
        IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }
        Dictionary<string, object> ExtensionData { get; set; }
    }
}
