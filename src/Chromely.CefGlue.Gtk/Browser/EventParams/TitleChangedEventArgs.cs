namespace Chromely.CefGlue.Gtk.Browser.EventParams
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class TitleChangedEventArgs : EventArgs
    {
        private readonly string _title;

        public TitleChangedEventArgs(string title)
        {
            _title = title;
        }

        public string Title
        {
            get { return _title; }
        }
    }
}
