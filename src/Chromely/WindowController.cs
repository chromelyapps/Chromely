// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <inheritdoc/>
public partial class WindowController : ChromelyWindowController
{
    protected readonly IChromelyRequestSchemeProvider _requestSchemeProvider;
    protected readonly ICefDownloader _binariesDownloader;

    /// <inheritdoc/>
    public WindowController(IChromelyWindow window,
                            IChromelyNativeHost nativeHost,
                            IChromelyConfiguration config,
                            IChromelyRouteProvider routeProvider,
                            IChromelyRequestHandler requestHandler,
                            IChromelyRequestSchemeProvider requestSchemeProvider,
                            ICefDownloader binariesDownloader,
                            ChromelyHandlersResolver handlersResolver)
        : base(window, nativeHost, config, routeProvider, requestHandler, handlersResolver)
    {
        // WindowController.NativeWindow
        _nativeHost.HostCreated += OnWindowCreated;
        _nativeHost.HostMoving += OnWindowMoving;
        _nativeHost.HostSizeChanged += OnWindowSizeChanged;
        _nativeHost.HostClose += OnWindowClose;

        _requestSchemeProvider = requestSchemeProvider;
        _binariesDownloader = binariesDownloader;
    }
}