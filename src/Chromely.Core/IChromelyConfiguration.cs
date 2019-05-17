using System;
using System.Collections.Generic;
using System.Reflection;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using Chromely.Core.Infrastructure;
using Chromely.Core.RestfulService;

namespace Chromely.Core
{
    public interface IChromelyConfiguration
    {
        string[] AppArgs { get; set; }
        List<Tuple<string, string, bool>> CommandLineArgs { get; set; }
        Dictionary<string, object> CustomSettings { get; set; }
        bool DebuggingMode { get; set; }
        ChromelyHostApi HostApi { get; set; }
        bool HostCenterScreen { get; set; }
        bool HostFrameless { get; set; }
        int HostHeight { get; set; }
        string HostIconFile { get; set; }
        WindowState HostState { get; set; }
        string HostTitle { get; set; }
        int HostWidth { get; set; }
        bool LoadCefBinariesIfNotFound { get; set; }
        string Locale { get; set; }
        string LogFile { get; set; }
        LogSeverity LogSeverity { get; set; }
        bool PerformDependencyCheck { get; set; }
        List<ControllerAssemblyInfo> ServiceAssemblies { get; }
        bool ShutdownCefOnExit { get; set; }
        bool SilentCefBinariesLoading { get; set; }
        string StartUrl { get; set; }
        bool StartWebSocket { get; set; }
        string WebsocketAddress { get; set; }
        int WebsocketPort { get; set; }
        IChromelyConfiguration RegisterEventHandler<T>(CefEventKey key, EventHandler<T> handler);
        IChromelyConfiguration RegisterEventHandler<T>(CefEventKey key, ChromelyEventHandler<T> handler);
        IChromelyConfiguration RegisterCustomHandler(CefHandlerKey key, Type implementation);
    }
}