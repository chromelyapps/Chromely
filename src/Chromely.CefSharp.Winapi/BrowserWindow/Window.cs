// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.BrowserWindow
{
    using System;
    using System.Linq;

    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.CefSharp.Winapi.Browser.Internals;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;

    using global::CefSharp;


    /// <summary>
    /// The window.
    /// </summary>
    public class Window : NativeWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase mApplication;

        /// <summary>
        /// The ChromiumWebBrowser object.
        /// </summary>
        private ChromiumWebBrowser mBrowser;

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
            mBrowser = new ChromiumWebBrowser(settings, hostConfig.StartUrl);
            mBrowser.IsBrowserInitializedChanged += IsBrowserInitializedChanged;

            // Set handlers
            mBrowser.SetEventHandlers();
            mBrowser.SetCustomHandlers();

            RegisterJsHandlers();
            mApplication = application;

            ShowWindow();
        }

        /// <summary>
        /// The browser.
        /// </summary>
        public ChromiumWebBrowser Browser => mBrowser;

        #region Dispose

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (mBrowser != null)
            {
                mBrowser.Dispose();
                mBrowser = null;
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
            mBrowser?.CreateBrowser(hwnd);
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
            mBrowser?.SetSize(width, height);
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        protected override void OnExit()
        {
            mApplication.Quit();
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
                mBrowser.SetSize(size.Width, size.Height);
                mBrowser.IsBrowserInitializedChanged -= IsBrowserInitializedChanged;
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
                            mBrowser.RegisterAsyncJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                        else
                        {
                            mBrowser.RegisterJsObject(handler.ObjectNameToBind, boundObject, options);
                        }
                    }
                }
            }
        }
    }
}
