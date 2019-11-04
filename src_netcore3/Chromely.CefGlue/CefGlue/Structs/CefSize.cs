// ReSharper disable ConvertToAutoProperty
#pragma warning disable 1591

// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xilium.CefGlue.Interop;

    public struct CefSize
    {
        private int _width;
        private int _height;

        public CefSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }
    }
}
