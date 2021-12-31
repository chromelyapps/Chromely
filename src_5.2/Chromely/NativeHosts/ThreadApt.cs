// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Threading;

namespace Chromely.NativeHost
{
    // https://github.com/dotnet/winforms/issues/5071
    public static class ThreadApt
    {
        public static void STA()
        {
            // Set STAThread 
#pragma warning disable CA1416 // Validate platform compatibility
            Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
#pragma warning restore CA1416 // Validate platform compatibility

        }
    }
}
