// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core.Host
{
    public class CreatedEventArgs : EventArgs
    {
        public CreatedEventArgs(IntPtr window, IntPtr winXID)
        {
            Window = window;
            WinXID = winXID;
        }

        public IntPtr Window { get; }
        public IntPtr WinXID { get; }
    }
}
