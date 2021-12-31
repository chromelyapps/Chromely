// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Xilium.CefGlue;

namespace Chromely.Browser
{
    public abstract partial class ChromiumBrowser
    {
        /// <summary>
        /// Returns true if the browser can navigate backwards.
        /// </summary>
        public bool CanGoBack
        {
            get { return Browser != null ? Browser.CanGoBack : false; }
        }

        /// <summary>
        /// Navigate backwards.
        /// </summary>
        public void GoBack()
        {
            Browser?.GoBack();
        }

        /// <summary>
        /// Returns true if the browser can navigate forwards.
        /// </summary>
        public bool CanGoForward
        {
            get { return Browser != null ? Browser.CanGoForward : false; }
        }

        /// <summary>
        /// Navigate forwards.
        /// </summary>
        public void GoForward()
        {
            Browser?.GoForward();
        }

        /// <summary>
        /// Returns true if the browser is currently loading.
        /// </summary>
        public bool IsLoading
        {
            get { return Browser != null ? Browser.IsLoading : false; }
        }

        /// <summary>
        /// Reload the current page.
        /// </summary>
        public void Reload()
        {
            Browser?.Reload();
        }

        /// <summary>
        /// Reload the current page ignoring any cached data.
        /// </summary>
        public void ReloadIgnoreCache()
        {
            Browser?.ReloadIgnoreCache();
        }

        /// <summary>
        /// Stop loading the page.
        /// </summary>
        public void StopLoad()
        {
            Browser?.StopLoad();
        }

        /// <summary>
        /// Returns the globally unique identifier for this browser. This value is also
        /// used as the tabId for extension APIs.
        /// </summary>
        public int Identifier
        {
            get { return Browser != null ? Browser.Identifier : 0; }
        }

        /// <summary>
        /// Returns true if the window is a popup window.
        /// </summary>
        public bool IsPopup
        {
            get { return Browser != null ? Browser.IsPopup : false; }
        }

        /// <summary>
        /// Returns true if a document has been loaded in the browser.
        /// </summary>
        public bool HasDocument
        {
            get { return Browser != null ? Browser.HasDocument : false; }
        }

        /// <summary>
        /// Returns the main (top-level) frame for the browser window.
        /// </summary>
        public CefFrame GetMainFrame()
        {
            return Browser?.GetMainFrame();
        }

        /// <summary>
        /// Returns the focused frame for the browser window.
        /// </summary>
        public CefFrame GetFocusedFrame()
        {
            return Browser?.GetFocusedFrame();
        }

        /// <summary>
        /// Returns the frame with the specified identifier, or NULL if not found.
        /// </summary>
        public CefFrame GetFrame(long identifier)
        {
            return Browser?.GetFrame(identifier);
        }

        /// <summary>
        /// Returns the frame with the specified name, or NULL if not found.
        /// </summary>
        public CefFrame GetFrame(string name)
        {
            return Browser?.GetFrame(name);
        }

        /// <summary>
        /// Returns the number of frames that currently exist.
        /// </summary>
        public int FrameCount
        {
            get { return Browser != null ? Browser.FrameCount : 0; }
        }

        /// <summary>
        /// Returns the identifiers of all existing frames.
        /// </summary>
        public long[] GetFrameIdentifiers()
        {
            return Browser?.GetFrameIdentifiers();
        }

        /// <summary>
        /// Returns the names of all existing frames.
        /// </summary>
        public string[] GetFrameNames()
        {
            return Browser?.GetFrameNames();
        }
    }
}
