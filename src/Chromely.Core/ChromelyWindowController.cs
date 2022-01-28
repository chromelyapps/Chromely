// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <summary>
/// Represents the window controller class.
/// </summary>
/// <remarks>
/// The class "Run" method launches the application.
/// </remarks>
public abstract class ChromelyWindowController : IDisposable
{
    protected IChromelyWindow _window;
    protected IChromelyNativeHost _nativeHost;
    protected IChromelyRouteProvider _routeProvider;
    protected IChromelyConfiguration _config;
    protected IChromelyRequestHandler _requestHandler;
    protected ChromelyHandlersResolver _handlersResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="ChromelyWindowController"/>.
    /// </summary>
    /// <param name="window">The main host window of type <see cref="IChromelyWindow" />.</param>
    /// <param name="nativeHost">The native host [Windows - win32, Linux - Gtk, MacOS - Cocoa] of type <see cref="IChromelyNativeHost"/>.</param>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="routeProvider">Instance of <see cref="IChromelyRouteProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromelyRequestHandler"/>.</param>
    /// <param name="handlersResolver">Instance of <see cref="ChromelyHandlersResolver"/>.</param>
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

    /// <summary>
    /// Gets the host window - instance of <see cref="IChromelyWindow"/>.
    /// </summary>
    public IChromelyWindow Window => _window;

    /// <summary>
    /// Gets the native host - instance of <see cref="IChromelyNativeHost"/>.
    /// </summary>
    public IChromelyNativeHost NativeHost => _nativeHost;

    /// <summary>
    /// Gets the route provider - instance of <see cref="IChromelyRouteProvider"/>.
    /// </summary>
    public IChromelyRouteProvider RouteProvider => _routeProvider;

    /// <summary>
    /// Gets the configuration - instance of <see cref="IChromelyConfiguration"/>.
    /// </summary>
    public IChromelyConfiguration Config => _config;

    /// <summary>
    /// Gets the request handler - instance of <see cref="IChromelyRequestHandler"/>.
    /// </summary>
    public IChromelyRequestHandler RequestHandler => _requestHandler;

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

    /// <summary>
    /// Exit the application.
    /// </summary>
    public abstract void Quit();

    #region Disposal

    private bool _disposed = false;

    /// <inheritdoc/>
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