using System;
using System.Collections.Generic;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.Core
{
    public class DefaultConfiguration : IChromelyConfiguration
    {
        public ChromelyPlatform Platform { get; set; }
        public string ChromelyVersion { get; set; }
        public bool LoadCefBinariesIfNotFound { get; set; }
        public bool SilentCefBinariesLoading { get; set; }
        public int WindowLeft { get; set; }
        public int WindowTop { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public bool WindowNoResize { get; set; }
        public bool WindowNoMinMaxBoxes { get; set; }
        public bool WindowFrameless { get; set; }
        public bool WindowCenterScreen { get; set; }
        public bool WindowKioskMode { get; set; }
        public WindowState WindowState { get; set; }
        public string WindowTitle { get; set; }
        public string WindowIconFile { get; set; }
        public WindowCustomStyle WindowCustomStyle { get; set; }
        public bool UseWindowCustomStyle { get; set; }
        public string AppExeLocation { get; set; }
        public string StartUrl { get; set; }
        public bool DebuggingMode { get; set; }
        public List<UrlScheme> UrlSchemes { get; set; }
        public List<ControllerAssemblyInfo> ControllerAssemblies { get; set; }
        public List<ChromelyEventHandler<object>> EventHandlers { get; set; }
        public List<Tuple<string, string>> CommandLineArgs { get; set; }
        public List<string> CommandLineOptions { get; set; }
        public Dictionary<string, string> CustomSettings { get; set; }
        public IChromelyJavaScriptExecutor JavaScriptExecutor { get; set; }
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}
