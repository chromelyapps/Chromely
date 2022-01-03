// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable CA1816

using System.Reflection;
using Xilium.CefGlue;

namespace Chromely.Browser
{
    public abstract partial class ChromiumBrowser
    {
        #region Disposal

        private bool _disposed;

        ~ChromiumBrowser()
        {
            Dispose(false);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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

            FreeUnmanagedResources();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void FreeUnmanagedResources()
        {
            unsafe
            {

                // Repeated if missed
                CefRuntime.Shutdown();

                // We don't want to change Xilium.CefGlue.CefBrowser
                // we check the internal _self property to see
                // if it is already destroyed
                if (Browser is not null
                    && typeof(CefBrowser).GetField("_self", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Browser) is Pointer self
                    && Pointer.Unbox(self) != null)
                {
                    var host = Browser.GetHost();
                    host.CloseBrowser(true);
                    host.Dispose();
                    Browser.Dispose();
                    Browser = null;
                    _browserWindowHandle = IntPtr.Zero;
                }
            }
        }

        #endregion
    }
}
