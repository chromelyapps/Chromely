#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Gtk.Browser
{
    using System;

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
