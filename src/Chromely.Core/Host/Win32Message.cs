using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Chromely.Core.Host
{
    public class Win32Message
    {
        public IntPtr Hwnd;
        public uint Value;
        public IntPtr WParam;
        public IntPtr LParam;
        public uint Time;
        public Point Point;
    }
}
