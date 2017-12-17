namespace Chromely.CefGlue.Gtk.Browser
{
    using Chromely.CefGlue.Gtk.Browser;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue;

    internal sealed class CefWebLoadHandler : CefLoadHandler
    {
        private readonly CefWebBrowser _core;

        public CefWebLoadHandler(CefWebBrowser core)
        {
            _core = core;
        }

        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            _core.OnLoadingStateChanged(isLoading, canGoBack, canGoForward);
        }
    }
}
