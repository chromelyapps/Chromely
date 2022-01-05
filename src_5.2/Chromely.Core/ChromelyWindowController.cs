// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

public abstract class ChromelyWindowController : IDisposable
{
    protected IChromelyWindow _window;
    protected IChromelyNativeHost _nativeHost;
    protected IChromelyRouteProvider _routeProvider;
    protected IChromelyConfiguration _config;
    protected IChromelyRequestHandler _requestHandler;
    protected ChromelyHandlersResolver _handlersResolver;

    public ChromelyWindowController(IChromelyWindow window,
                                    IChromelyNativeHost nativeHost,
                                    IChromelyConfiguration config,
                                    IChromelyRouteProvider routeProvider,
                                    IChromelyRequestHandler requestHandler,
                                    ChromelyHandlersResolver handlersResolver)
    {
        _window = window;
        _nativeHost = nativeHost;
        _config = config;
        _routeProvider = routeProvider;
        _requestHandler = requestHandler;
        _handlersResolver = handlersResolver;
    }

    public IChromelyWindow Window => _window;
    public IChromelyNativeHost NativeHost => _nativeHost;
    public IChromelyRouteProvider RouteProvider => _routeProvider;
    public IChromelyConfiguration Config => _config;
    public IChromelyRequestHandler RequestTaskRunner => _requestHandler;

    #region Destructor

    /// <summary>
    /// Finalizes an instance of the <see cref="ChromelyWindowController"/> class. 
    /// </summary>
    ~ChromelyWindowController()
    {
        Dispose(false);
    }

    #endregion Destructor

    /// <summary>
    /// Runs the application.
    /// This call does not return until the application terminates
    /// or an error is occurred.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    ///  0 successfully run application - now terminated
    ///  1 on internal exception (see log for more information).
    /// </returns>
    public abstract int Run(string[] args);

    public abstract void Quit();

    #region Disposal

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        // If there are managed resources
        if (disposing)
        {
        }

        _nativeHost?.Dispose();
        _window?.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}