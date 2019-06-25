// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CefSharp;
using Chromely.CefSharp.Winapi.Browser;
using Chromely.CefSharp.Winapi.Browser.Handlers;
using Chromely.CefSharp.Winapi.Browser.Internals;
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
        private IntPtr _browserWndProc;
        private IntPtr _browserRenderWidgetHandle;
        private IntPtr _browserRenderWidgetWndProc;
        private List<WndProcOverride> _wndProcOverrides = new List<WndProcOverride>();

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
            _application.Quit();
        }

        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private struct ChildWindow
        {
            public IntPtr Handle;
            public string ClassName;
        }

        private class EnumChildWindowsDetails
        {
            public List<ChildWindow> Windows = new List<ChildWindow>();
        }

        private class WndProcOverride
        {
            private IntPtr handle;
            private IntPtr originalWndProc;
            private WindowProc newWndProc;

            public WndProcOverride(IntPtr wndHandle, string wndClassName)
            {
                handle = wndHandle;
                newWndProc = new WindowProc(OverridenWndProc);
                originalWndProc = User32Methods.SetWindowLongPtr(handle, -4, Marshal.GetFunctionPointerForDelegate(newWndProc));
            }

            private IntPtr OverridenWndProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
            {
                var msg = (WM)uMsg;
                var originalRet = User32Methods.CallWindowProc(originalWndProc, hWnd, uMsg, wParam, lParam);
                switch (msg)
                {
                    case WM.NCHITTEST:
                        {
                            // If we hit a non client area (our overlapped frame) then we'll force an NCHITTEST to bubble back up to the main process
                            IntPtr hitTest = HitTestNCA(hWnd, wParam, lParam);
                            if (hitTest != IntPtr.Zero)
                            {
                                return new IntPtr(-1);
                            }
                            break;
                        }
                }
                return originalRet;
            }
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

                if (_hostConfig.HostFrameless)
                {
                    var childWindowsDetails = new EnumChildWindowsDetails();
                    var gcHandle = GCHandle.Alloc(childWindowsDetails);
                    EnumChildWindows(Handle, new EnumWindowProc(EnumWindow), GCHandle.ToIntPtr(gcHandle));

                    foreach (ChildWindow childWindow in childWindowsDetails.Windows)
                    {
                        var wndProcOverride = new WndProcOverride(childWindow.Handle, childWindow.ClassName);
                        _wndProcOverrides.Add(wndProcOverride);
                    }

                    gcHandle.Free();
                }
            }
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            var buffer = new StringBuilder(128);
            User32Methods.GetClassName(hWnd, buffer, buffer.Capacity);

            var childWindow = new ChildWindow()
            {
                Handle = hWnd,
                ClassName = buffer.ToString()
            };

            var gcHandleDetails = GCHandle.FromIntPtr(lParam);
            var details = (EnumChildWindowsDetails)gcHandleDetails.Target;
            details.Windows.Add(childWindow);

            return true;
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
