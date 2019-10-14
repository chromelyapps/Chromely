#pragma warning disable 1591

// ReSharper disable once CheckNamespace
namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class CefVersionMismatchException : CefRuntimeException
    {
        public CefVersionMismatchException(string message)
            : base(message)
        {
        }
    }
}
