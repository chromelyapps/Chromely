// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Network;
using Xilium.CefGlue;

namespace Chromely.Browser
{

    internal interface IDefaultCustomHandler
    {
    }

    internal interface IDefaultResourceCustomHandler
    {
    }


    internal interface IDefaultAssemblyResourceCustomHandler
    {
    }

    internal interface IDefaultRequestCustomHandler
    {
    }

    internal interface IDefaultExernalRequestCustomHandler
    {
    }

    /*
     * Message Router
     */
    internal sealed class ChromelyMessageRouter : DefaultMessageRouterHandler
    {
        public ChromelyMessageRouter(IChromelyRouteProvider routeProvider, IChromelyRequestHandler requestHandler, IChromelyDataTransferOptions dataTransferOptions, IChromelyErrorHandler chromelyErrorHandler) 
            : base(routeProvider, requestHandler, dataTransferOptions, chromelyErrorHandler)
        {
        }
    }

    /*
     * Resource/Request Scheme handlers
     */
    internal sealed class ChromelyResourceSchemeHandlerFactory : DefaultResourceSchemeHandlerFactory, IDefaultResourceCustomHandler
    {
        public ChromelyResourceSchemeHandlerFactory(IChromelyConfiguration config, IChromelyErrorHandler chromelyErrorHandler) : base(config, chromelyErrorHandler)
        {
        }
    }

    internal sealed class ChromelyAssemblyResourceSchemeHandlerFactory : DefaultAssemblyResourceSchemeHandlerFactory, IDefaultAssemblyResourceCustomHandler
    {
        public ChromelyAssemblyResourceSchemeHandlerFactory(IChromelyConfiguration config, IChromelyErrorHandler chromelyErrorHandler) : base(config, chromelyErrorHandler)
        {
        }
    }

    internal sealed class ChromelyRequestSchemeHandlerFactory : DefaultRequestSchemeHandlerFactory, IDefaultRequestCustomHandler
    {
        public ChromelyRequestSchemeHandlerFactory(IChromelyRouteProvider routeProvider, IChromelyRequestSchemeProvider requestSchemeProvider, IChromelyRequestHandler requestHandler, IChromelyDataTransferOptions dataTransferOptions, IChromelyErrorHandler chromelyErrorHandler) 
            : base(routeProvider, requestSchemeProvider, requestHandler, dataTransferOptions, chromelyErrorHandler)
        {
        }
    }

    internal sealed class ChromelyExternalRequestSchemeHandlerFactory : DefaultExternalRequestSchemeHandlerFactory, IDefaultExernalRequestCustomHandler
    {
    }

    /*
     * Custom handlers
     */
    internal sealed class ChromelyContextMenuHandler : DefaultContextMenuHandler, IDefaultCustomHandler
    {
        public ChromelyContextMenuHandler(IChromelyConfiguration config) : base(config)
        {
        }
    }

    internal sealed class ChromelyDisplayHandler : DefaultDisplayHandler, IDefaultCustomHandler
    {
        public ChromelyDisplayHandler(IChromelyConfiguration config, IChromelyWindow window) : base(config, window)
        {
        }
    }

    internal sealed class ChromelyLoadHandler : DefaultLoadHandler, IDefaultCustomHandler
    {
        public ChromelyLoadHandler(IChromelyConfiguration config, IChromelyWindow window) : base(config, window)
        {
        }
    }

    internal sealed class ChromelyDownloadHandler : DefaultDownloadHandler, IDefaultCustomHandler
    {
    }

    internal sealed class ChromelyDragHandler : DefaultDragHandler, IDefaultCustomHandler
    {
        public ChromelyDragHandler(IChromelyConfiguration config) : base(config)
        {
        }
    }

    internal sealed class ChromelyLifeSpanHandler : DefaultLifeSpanHandler, IDefaultCustomHandler
    {
        public ChromelyLifeSpanHandler(IChromelyConfiguration config, IChromelyRequestHandler requestHandler, IChromelyRouteProvider routeProvider, IChromelyWindow window)
            :base(config, requestHandler, routeProvider, window)
        {
        }
    }

    internal sealed class ChromelyRequestHandler : DefaultRequestHandler, IDefaultCustomHandler
    {
        public ChromelyRequestHandler(IChromelyConfiguration config, IChromelyRequestHandler requestHandler, IChromelyRouteProvider routeProvider, IChromelyWindow window, CefResourceRequestHandler resourceRequestHandler = null)
                : base(config, requestHandler, routeProvider, window, resourceRequestHandler)
        {
        }
    }

    public interface IDefaultOwinCustomHandler
    {
    }
}
