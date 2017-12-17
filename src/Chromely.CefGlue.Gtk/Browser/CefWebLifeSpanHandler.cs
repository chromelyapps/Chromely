namespace Chromely.CefGlue.Gtk.Browser
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue;

    internal sealed class CefWebLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly CefWebBrowser _core;

        public CefWebLifeSpanHandler(CefWebBrowser core)
        {
            _core = core;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            _core.OnCreated(browser);
        }

        protected override bool DoClose(CefBrowser browser)
        {
            // TODO: dispose core
            return false;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
           // DemoApp.BrowserMessageRouter.OnBeforeClose(browser);
        }
    }
}
