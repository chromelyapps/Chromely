// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyConfiguration.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Chromely.Core.Configuration;
using Chromely.Core.Infrastructure;
using Chromely.Core.Network;

namespace Chromely.Core
{
    public interface IChromelyConfiguration
    {
        string AppExeLocation { get; set; }
        string AppName { get; set; }
        string ChromelyVersion { get; set; }
        IDictionary<string, string> CommandLineArgs { get; set; }
        List<string> CommandLineOptions { get; set; }
        List<ControllerAssemblyInfo> ControllerAssemblies { get; set; }
        IDictionary<string, string> CustomSettings { get; set; }
        bool DebuggingMode { get; set; }
        string DevToolsUrl { get; set; }
        List<ChromelyEventHandler<object>> EventHandlers { get; set; }
        IDictionary<string, object> ExtensionData { get; set; }
        IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }
        bool LoadCefBinariesIfNotFound { get; set; }
        ChromelyPlatform Platform { get; set; }
        bool SilentCefBinariesLoading { get; set; }
        string StartUrl { get; set; }
        List<UrlScheme> UrlSchemes { get; set; }
        IWindowOptions WindowOptions { get; set; }
    }
}