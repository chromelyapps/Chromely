// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;

using CefSharp;
using Chromely.CefSharp.Winapi.Browser;
using Chromely.CefSharp.Winapi.Browser.Handlers;
using Chromely.CefSharp.Winapi.Browser.Internals;
using Chromely.Common;
using Chromely.Core;
using Chromely.Core.Infrastructure;
using WinApi.User32;

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    /// <summary>
    /// The window.
    /// </summary>
    internal class Window : NativeWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase _application;

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration _hostConfig;

        private ChromeWidgetMessageInterceptor _interceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public Window(HostBase application, ChromelyConfiguration hostConfig, CefSettings settings)
            : base(hostConfig)
        {
            _hostConfig = hostConfig;
            Browser = new ChromiumWebBrowser(settings, _hostConfig.StartUrl);
            Browser.IsBrowserInitializedChanged += IsBrowserInitializedChanged;

            // Set handlers
            Browser.SetEventHandlers();
            Browser.SetCustomHandlers();

            RegisterJsHandlers();
            _application = application;

            ShowWindow();
        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        public ChromiumWebBrowser Browser { get; private set; }

        #region Dispose

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (Browser != null)
            {
                Browser.Dispose();
                Browser = null;
            }
        }

        #endregion Dispose

        /// <summary>
        /// The on realized.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Exception returned for MacOS not supported.
        /// </exception>
        protected override void OnCreate(IntPtr hwnd, int width, int height)
        {
            Browser?.CreateBrowser(hwnd, _hostConfig.StartUrl);
        }

        /// <summary>
        /// The on moving.
        /// </summary>
        protected override void OnMoving()
        {
            if (Browser != null && Browser.IsBrowserInitialized)
            {
                Browser.GetBrowser().GetHost().NotifyMoveOrResizeStarted();
            }
        }

        /// <summary>
        /// The on size.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        protected override void OnSize(int width, int height)
        {
            Browser?.SetSize(width, height);
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        protected override void OnExit()
        {
        }

        /// <summary>
        /// Browser initialized changed event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs eventArgs)
        {
            if (eventArgs.IsBrowserInitialized)
            {
                var size = GetClientSize();
                Browser.SetSize(size.Width, size.Height);
                Browser.IsBrowserInitializedChanged -= IsBrowserInitializedChanged;
                Browser.GetBrowserHost().GetWindowHandle();

                if (_hostConfig.HostPlacement.Frameless && _hostConfig.HostPlacement.FramelessOptions.IsDraggable)
                {
                    ChromeWidgetMessageInterceptor.Setup(_interceptor, Handle, _hostConfig.HostPlacement.FramelessOptions, (message) =>
                    {
                        var msg = (WM)message.Value;
                        switch (msg)
                        {
                            case WM.LBUTTONDOWN:
                                User32Methods.ReleaseCapture();
                                NativeMethods.SendMessage(Handle, (int)WM.SYSCOMMAND, NativeMethods.SC_DRAGMOVE, 0);
                                break;
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Registers custom Javascript Bound (JSB) handlers.
        /// </summary>
        private void RegisterJsHandlers()
        {
            // Register javascript handlers
            var jsHandlerObjs = IoC.GetAllInstances(typeof(ChromelyJsHandler));
            if (jsHandlerObjs != null)
            {
                var jsHandlers = jsHandlerObjs.ToList();

                foreach (var item in jsHandlers)
                {
                    if (item is ChromelyJsHandler handler)
                    {
                        BindingOptions options = null;

                        if (handler.BindingOptions is BindingOptions)
                        {
                            options = (BindingOptions)handler.BindingOptions;
                        }

                        var boundObject = handler.UseDefault ? new CefSharpBoundObject() : handler.BoundObject;

                        if (handler.RegisterAsAsync)
                        {
                            Browser.RegisterAsyncJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                        else
                        {
                            Browser.RegisterJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                    }
                }
            }
        }
    }
}
