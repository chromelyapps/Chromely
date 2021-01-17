// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Host;
using Chromely.Core.Network;
using System;

namespace Chromely.Core
{
    public abstract class ChromelyWindowController : IDisposable
    {
        protected IChromelyWindow _window;
        protected IChromelyNativeHost _nativeHost;
        protected IChromelyRouteProvider _routeProvider;
        protected IChromelyConfiguration _config;
        protected IChromelyRequestTaskRunner _requestTaskRunner;
        protected IChromelyCommandTaskRunner _commandTaskRunner;
        protected ChromelyHandlersResolver _handlersResolver;

        public ChromelyWindowController(IChromelyWindow window, 
                                        IChromelyNativeHost nativeHost, 
                                        IChromelyConfiguration config, 
                                        IChromelyRouteProvider routeProvider, 
                                        IChromelyRequestTaskRunner requestTaskRunner, 
                                        IChromelyCommandTaskRunner commandTaskRunner,
                                        ChromelyHandlersResolver handlersResolver)
        {
            _window = window;
            _nativeHost = nativeHost;
            _config = config;
            _routeProvider = routeProvider;
            _requestTaskRunner = requestTaskRunner;
            _commandTaskRunner = commandTaskRunner;
            _handlersResolver = handlersResolver;
        }

        public IChromelyWindow Window => _window;
        public IChromelyNativeHost NativeHost => _nativeHost;
        public IChromelyRouteProvider RouteProvider => _routeProvider;
        public IChromelyConfiguration Config => _config;
        public IChromelyRequestTaskRunner RequestTaskRunner => _requestTaskRunner;
        public IChromelyCommandTaskRunner CommandTaskRunner => _commandTaskRunner;

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
}
