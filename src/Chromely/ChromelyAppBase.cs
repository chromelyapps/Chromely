// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Browser;
using Chromely.Core;
using Chromely.Core.Defaults;
using Chromely.Core.Host;
using Chromely.Core.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xilium.CefGlue;

namespace Chromely
{
    public abstract class ChromelyAppBase : ChromelyApp
    {
        public override void ConfigureCoreServices(IServiceCollection services)
        {
            base.ConfigureCoreServices(services);

            // Add window core services if not already added.
            // Expected window core services are -
            // IChromelyNativeHost, IChromelyWindow

            services.TryAddSingleton<IChromelyInfo, ChromelyInfo>();
            services.TryAddSingleton<IChromelyRouteProvider, DefaultRouteProvider>();
            services.TryAddSingleton<IChromelyRequestTaskRunner, DefaultRequestTaskRunner>();
            services.TryAddSingleton<IChromelyCommandTaskRunner, DefaultCommandTaskRunner>();

            services.TryAddSingleton<IChromelyWindow, Window>();
            services.TryAddSingleton<ChromelyWindowController, WindowController>();
        }

        public sealed override void ConfigureDefaultHandlers(IServiceCollection services)
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
}
