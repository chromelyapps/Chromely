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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Chromely.CefSharp.Winapi.Browser;
    using Chromely.CefSharp.Winapi.Browser.Handlers;
    using Chromely.CefSharp.Winapi.Browser.Internals;
    using Chromely.Core;
    using Chromely.Core.Infrastructure;

    using global::CefSharp;
    using NetCoreEx.Geometry;
    using WinApi.DwmApi;
    using WinApi.User32;


    /// <summary>
    /// The window.
    /// </summary>
    internal class Window : NativeWindow
    {
        /// <summary>
        /// The host/app/window application.
        /// </summary>
        private readonly HostBase mApplication;

        /// <summary>
        /// The ChromiumWebBrowser object.
        /// </summary>
        private ChromiumWebBrowser mBrowser;

        private IntPtr mBrowserHandle;
        private IntPtr mBrowserWndProc;
        private IntPtr mBrowserRenderWidgetHandle;
        private IntPtr mBrowserRenderWidgetWndProc;
        private List<ChildWindow> childWindows;
        private List<WndProcOverride> wndProcOverrides = new List<WndProcOverride>();

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
            mBrowser.DragHandler = new CefSharpDragHandler();

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
            mBrowser?.CreateBrowser(hwnd, HostConfig.StartUrl);
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
            private ChromiumWebBrowser browser;
            private IntPtr mainHandle;
            private IntPtr handle;
            private IntPtr originalWndProc;
            private string className;
            private WindowProc newWndProc;

            public WndProcOverride(IntPtr wndHandle, string wndClassName, ChromiumWebBrowser mBrowser, IntPtr parentHandle)
            {
                browser = mBrowser;
                mainHandle = parentHandle;
                handle = wndHandle;
                className = wndClassName;
                newWndProc = new WindowProc(OverridenWndProc);
                originalWndProc = User32Methods.SetWindowLongPtr(handle, -4, Marshal.GetFunctionPointerForDelegate(newWndProc));
            }

            private IntPtr OverridenWndProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
            {
                var msg = (WM)uMsg;
                var originalRet = User32Methods.CallWindowProc(originalWndProc, hWnd, uMsg, wParam, lParam);
                switch (msg)
                {
                    case WM.LBUTTONDOWN:
                        {
                            var mousePoint = new System.Drawing.Point(lParam.ToInt32());
                            var dragHandler = browser.DragHandler as CefSharpDragHandler;
                            if (dragHandler.DragRegion.IsVisible(mousePoint)) {
                                // Release the capture of browser window and send a caption drag to the main window
                                User32Methods.ReleaseCapture();
                                User32Methods.SendMessage(mainHandle, (uint)WM.NCLBUTTONDOWN, new IntPtr(2), IntPtr.Zero);
                            }
                            break;
                        }

                    case WM.NCHITTEST:
                        {
                            // If we hit a non client area (our overlapped frame) then we'll force an NCHITTEST to bubble back up to the main process
                            IntPtr hitTest = HitTestNCA(hWnd, wParam, lParam);
                            if (hitTest != IntPtr.Zero) {
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
                mBrowser.SetSize(size.Width, size.Height);
                mBrowser.IsBrowserInitializedChanged -= IsBrowserInitializedChanged;

                mBrowserHandle = mBrowser.GetBrowserHost().GetWindowHandle();

                var childWindowsDetails = new EnumChildWindowsDetails();
                var gcHandle = GCHandle.Alloc(childWindowsDetails);
                EnumChildWindows(Handle, new EnumWindowProc(EnumWindow), GCHandle.ToIntPtr(gcHandle));

                foreach(ChildWindow childWindow in childWindowsDetails.Windows)
                {
                    var wndProcOverride = new WndProcOverride(childWindow.Handle, childWindow.ClassName, mBrowser, Handle);
                    wndProcOverrides.Add(wndProcOverride);
                }

                childWindows = childWindowsDetails.Windows;

                gcHandle.Free();
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
