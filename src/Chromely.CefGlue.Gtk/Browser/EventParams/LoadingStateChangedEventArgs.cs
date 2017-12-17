namespace Chromely.CefGlue.Gtk.Browser.EventParams
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class LoadingStateChangedEventArgs : EventArgs
    {
        private readonly bool _isLoading;
        private readonly bool _canGoBack;
        private readonly bool _canGoForward;

        public LoadingStateChangedEventArgs(bool isLoading, bool canGoBack, bool canGoForward)
        {
            _isLoading = isLoading;
            _canGoBack = canGoBack;
            _canGoForward = canGoForward;
        }

        public bool Loading
        {
            get { return _isLoading; }
        }

        public bool CanGoBack
        {
            get { return _canGoBack; }
        }

        public bool CanGoForward
        {
            get { return _canGoForward; }
        }
    }
}
