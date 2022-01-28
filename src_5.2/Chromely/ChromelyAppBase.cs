// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <inheritdoc/>
public abstract class ChromelyAppBase : ChromelyApp
{
    /// <inheritdoc/>
    public override void ConfigureCoreServices(IServiceCollection services)
    {
        base.ConfigureCoreServices(services);

        // Add window core services if not already added.

        services.TryAddSingleton<IChromelyInfo, ChromelyInfo>();
        services.TryAddSingleton<IChromelyRouteProvider, DefaultRouteProvider>();
        services.TryAddSingleton<IChromelyDataTransferOptions, DataTransferOptions>();
        services.TryAddSingleton<IChromelyModelBinder, DefaultModelBinder>();
        services.TryAddSingleton<IChromelyRequestHandler, DefaultActionRequestHandler>();
        services.TryAddSingleton<ICefDownloader, DefaultCefDownloader>();

        services.TryAddSingleton<IChromelyWindow, Window>();
        services.TryAddSingleton<ChromelyWindowController, WindowController>();

        var platform = ChromelyRuntime.Platform;

        switch (platform)
        {
            case ChromelyPlatform.MacOSX:
                services.TryAddSingleton<IChromelyNativeHost, ChromelyMacHost>();
                break;

            case ChromelyPlatform.Linux:
                services.TryAddSingleton<IChromelyNativeHost, ChromelyLinuxHost>();
                break;

            case ChromelyPlatform.Windows:
                services.TryAddSingleton<IWindowMessageInterceptor, DefaultWindowMessageInterceptor>();
                services.TryAddSingleton<IKeyboadHookHandler, DefaulKeyboadHookHandler>();
                services.TryAddSingleton<IChromelyNativeHost, ChromelyWinHost>();
                break;

            default:
                services.TryAddSingleton<IWindowMessageInterceptor, DefaultWindowMessageInterceptor>();
                services.TryAddSingleton<IKeyboadHookHandler, DefaulKeyboadHookHandler>();
                services.TryAddSingleton<IChromelyNativeHost, ChromelyWinHost>();
                break;
        }
    }

    /// <inheritdoc/>
    public override void ConfigureDefaultHandlers(IServiceCollection services)
    {
        base.ConfigureDefaultHandlers(services);

        services.AddSingleton<IChromelyMessageRouter, ChromelyMessageRouter>();

        // Add default resource/request handlers
        services.AddSingleton<IChromelyRequestSchemeProvider, DefaultRequestSchemeProvider>();

        services.AddSingleton<CefSchemeHandlerFactory, ChromelyResourceSchemeHandlerFactory>();
        services.AddSingleton<CefSchemeHandlerFactory, ChromelyAssemblyResourceSchemeHandlerFactory>();
        services.AddSingleton<CefSchemeHandlerFactory, ChromelyRequestSchemeHandlerFactory>();
        services.AddSingleton<CefSchemeHandlerFactory, ChromelyExternalRequestSchemeHandlerFactory>();

        // Adde default custom handlers
        services.AddSingleton<CefContextMenuHandler, ChromelyContextMenuHandler>();
        services.AddSingleton<CefDisplayHandler, ChromelyDisplayHandler>();
        services.AddSingleton<CefDownloadHandler, ChromelyDownloadHandler>();
        services.AddSingleton<CefDragHandler, ChromelyDragHandler>();
        services.AddSingleton<CefLifeSpanHandler, ChromelyLifeSpanHandler>();
        services.AddSingleton<CefLoadHandler, ChromelyLoadHandler>();
        services.AddSingleton<CefRequestHandler, ChromelyRequestHandler>();
    }
}