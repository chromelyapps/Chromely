namespace Chromely.CefGlue.Gtk.Browser.EventParams
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class TargetUrlChangedEventArgs : EventArgs
    {
        private readonly string _targetUrl;

        public TargetUrlChangedEventArgs(string targetUrl)
        {
            _targetUrl = targetUrl;
        }

        public string TargetUrl
        {
            get { return _targetUrl; }
        }
    }
}
