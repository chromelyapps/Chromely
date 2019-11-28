using System;

namespace Chromely.Core.Host
{
    public class CreatedEventArgs : EventArgs
    {
        public CreatedEventArgs(IntPtr app, IntPtr window, IntPtr winXID)
        {
            App = app;
            Window = window;
            WinXID = winXID;
        }

        public IntPtr App { get; }
        public IntPtr Window { get; }
        public IntPtr WinXID { get; }
    }
}
