// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Browser;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Network;
using System.Linq;

namespace Chromely
{
    public partial class WindowController : ChromelyWindowController
    {
        protected IChromelyRequestSchemeProvider _requestSchemeProvider;
        protected ICefBinariesDownloader _binariesDownloader;

        public WindowController(IChromelyWindow window, 
                                IChromelyNativeHost nativeHost, 
                                IChromelyConfiguration config, 
                                IChromelyRouteProvider routeProvider,
                                IChromelyRequestHandler requestHandler, 
                                IChromelyRequestSchemeProvider requestSchemeProvider,
                                ChromelyHandlersResolver handlersResolver)
            : base(window, nativeHost, config, routeProvider, requestHandler, handlersResolver)
        {
            // WindowController.NativeWindow
            _nativeHost.HostCreated += OnWindowCreated;
            _nativeHost.HostMoving += OnWindowMoving;
            _nativeHost.HostSizeChanged += OnWindowSizeChanged;
            _nativeHost.HostClose += OnWindowClose;

            _requestSchemeProvider = requestSchemeProvider;

            // Set CefBinariesDownloader
            var objList = _handlersResolver?.Invoke(typeof(ICefBinariesDownloader));
            _binariesDownloader = objList?.FirstOrDefault() as ICefBinariesDownloader;
        }
    }
}
