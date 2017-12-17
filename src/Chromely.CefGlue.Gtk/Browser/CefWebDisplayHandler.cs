namespace Chromely.CefGlue.Gtk.Browser
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue;

    internal sealed class CefWebDisplayHandler : CefDisplayHandler
    {
        private readonly CefWebBrowser _core;

        public CefWebDisplayHandler(CefWebBrowser core)
        {
            _core = core;
        }

        protected override void  OnTitleChange(CefBrowser browser, string title)
        {
            _core.OnTitleChanged(title);
        }

        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
            if (frame.IsMain)
            {
                _core.OnAddressChanged(url);
            }
        }

        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
            _core.OnTargetUrlChanged(value);
        }

        protected override bool OnTooltip(CefBrowser browser, string text)
        {
        	Console.WriteLine("OnTooltip: {0}", text);
        	return false;
        }
    }
}
